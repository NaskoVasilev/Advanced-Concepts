using System;
using TemporalTablesDemo.Data.Common;

namespace TemporalTablesDemo.Services.Models
{
    public class HistoryOf<T> where T : Entity, ITemporalEntity
    {
        public T Entry { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public string Changes { get; set; }
    }
}
