using System.Collections;

namespace Flex.Core.Extensions.CommonExtensions
{
    public static class HashTableExtension
    {
        public static void SetValue(this IDictionary hash, string name, object value)
        {
            if(hash.Contains(name))
                hash[name] = value;
            else
                hash.Add(name, value);
        }
        public static string GetStringValue(this IDictionary hash, string name)
        {
            if (!hash.Contains(name))
                return null;
            else
                return hash[name].ToString();
        }
        public static object GetValue(this IDictionary hash, string name)
        {
            if (!hash.Contains(name))
                return null;
            else
                return hash[name];
        }
    }
}
