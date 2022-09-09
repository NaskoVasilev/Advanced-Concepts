using System.Collections.Generic;
using TemporalTablesDemo.Data.Common;

namespace TemporalTablesDemo.Data.Models
{
    public class Company : Entity, ITemporalEntity
    {
        public string Name { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
