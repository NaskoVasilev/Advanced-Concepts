using TemporalTablesDemo.Data.Common;

namespace TemporalTablesDemo.Data.Models
{
    public class Employee : ITemporalEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int Test { get; set; }

        public override string ToString()
        {
            return $"{this.Id}, {this.FirstName} {this.LastName} working in {this.Department}";
        }
    }
}
