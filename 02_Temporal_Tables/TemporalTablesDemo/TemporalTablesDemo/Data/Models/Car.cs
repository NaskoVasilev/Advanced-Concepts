using TemporalTablesDemo.Data.Common;

namespace TemporalTablesDemo.Data.Models
{
    public class Car : Entity, ITemporalEntity
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public Engine Engine { get; set; }
    }
}
