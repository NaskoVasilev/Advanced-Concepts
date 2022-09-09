using System.Collections.Generic;
using System.Linq;
using TemporalTablesDemo.Data.Common;
using TemporalTablesDemo.Services.Models;

namespace TemporalTablesDemo.Services
{
    public interface IHistoryService
    {
        IQueryable<HistoryOf<T>> GetHistoryById<T>(int id) where T : Entity, ITemporalEntity;
    }
}
