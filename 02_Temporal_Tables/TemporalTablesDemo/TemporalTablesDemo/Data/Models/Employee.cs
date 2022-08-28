namespace TemporalTablesDemo.Data.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public override string ToString()
        {
            return $"{this.Id}, {this.FirstName} {this.LastName} working in {this.Department}";
        }
    }
}
