using System.Collections.Generic;
using TemporalTablesDemo.Data.Common;

namespace TemporalTablesDemo.Data.Models
{
    public class Company : ITemporalEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
