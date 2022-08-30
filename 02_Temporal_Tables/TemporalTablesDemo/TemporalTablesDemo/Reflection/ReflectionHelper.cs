using System.Collections.Generic;
using TemporalTablesDemo.Data.Common;

namespace TemporalTablesDemo.Reflection
{
    public class ReflectionHelper
    {
        public static IEnumerable<PropertyChange> GetChanges<T>(T current, T previous, IEnumerable<string> properties)
        {
            foreach (var prop in properties)
            {
                yield return new PropertyChange
                {
                    CurrentValue = GetValue(current, prop),
                    PreviousValue = GetValue(previous, prop),
                    Name = prop
                };
            }
        }

        public static object GetValue(object obj, string property) 
            => obj.GetType().GetProperty(property).GetValue(obj, null);
    }
}
