using System.ComponentModel;

namespace Flex.Core.Extensions
{
    public static class EnumExension
    {
        public static string GetEnumDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
