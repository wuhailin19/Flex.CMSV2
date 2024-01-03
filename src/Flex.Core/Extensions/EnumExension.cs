using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Flex.Core.Extensions
{
    public static class EnumExension
    {
        public static ConcurrentDictionary<Enum,string> keyValuePairs 
            = new ConcurrentDictionary<Enum,string>();
        public static string GetEnumDescription(this Enum value)
        {
            if (keyValuePairs.TryGetValue(value, out var result))
            {
                return result;
            }
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
            var newValue = attribute == null ? value.ToString() : attribute.Description;
            keyValuePairs.AddOrUpdate(value, newValue, (existingKey, existingValue) => newValue);
            return newValue;
        }
    }
}
