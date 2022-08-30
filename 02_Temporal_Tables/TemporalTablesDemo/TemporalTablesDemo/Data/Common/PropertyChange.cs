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

        public string Name { get; set; }

        public object PreviousValue { get; set; }

        public object CurrentValue { get; set; }
    }
}
