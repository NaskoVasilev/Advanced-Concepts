using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Principal;
using TemporalTablesDemo.Data;
using TemporalTablesDemo.Data.Common;
using TemporalTablesDemo.Data.Interceptors;
using TemporalTablesDemo.Data.Models;
using TemporalTablesDemo.Services.Models;

namespace TemporalTablesDemo.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly CompanyDbContext context;

        public HistoryService(CompanyDbContext context)
        {
            this.context = context;
        }

        public IQueryable<HistoryOf<T>> GetHistoryById<T>(int id) where T : Entity, ITemporalEntity
        {
            var result = context.Set<T>().TemporalAsOf(DateTime.MinValue).Where(e => e.Id == id)
                 .OrderByDescending(e => EF.Property<DateTime>(e, ModelConstants.PeriodStartColumnName))
                 .Select(e => new HistoryOf<T>
                 {
                     Entry = e,
                     PeriodStart = EF.Property<DateTime>(e, ModelConstants.PeriodStartColumnName),
                     PeriodEnd = EF.Property<DateTime>(e, ModelConstants.PeriodEndColumnName),
                     Changes = EF.Property<string>(e, ModelConstants.ChangesColumnName),
                 });
            return result;
        }

        //public IQueryable<HistoryOf<T>> GetHistoryById<T>(int id) where T : Entity, ITemporalEntity
        //{
        //    var result = context.Set<T>()
        //        .AsNoTracking()
        //        .Where(e => e.Id == id)
        //        .OrderByDescending(e => EF.Property<DateTime>(e, ModelConstants.PeriodStartColumnName))
        //        .TagWith(nameof(HistoryQueryInterceptor))
        //        .Select(e => new HistoryOf<T>
        //        {
        //            Entry = e,
        //            PeriodStart = EF.Property<DateTime>(e, ModelConstants.PeriodStartColumnName),
        //            PeriodEnd = EF.Property<DateTime>(e, ModelConstants.PeriodEndColumnName),
        //            Changes = EF.Property<string>(e, ModelConstants.ChangesColumnName),
        //        });

        //    return result;
        //}
    }
}
