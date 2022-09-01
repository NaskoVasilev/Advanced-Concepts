using System.Text.Json.Serialization;

namespace TemporalTablesDemo.Data.Common
{
    public class PropertyChange
    {
        public PropertyChange()
        {
        }

        public PropertyChange(string name, object previousValue, object currentValue)
        {
            Name = name;
            PreviousValue = previousValue;
            CurrentValue = currentValue;
        }

        [JsonPropertyName("N")]
        public string Name { get; set; }

        [JsonPropertyName("P")]
        public object PreviousValue { get; set; }

        [JsonPropertyName("C")]
        public object CurrentValue { get; set; }
    }
}
