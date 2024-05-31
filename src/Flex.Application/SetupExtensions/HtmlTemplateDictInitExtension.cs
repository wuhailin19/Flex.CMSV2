using Flex.Domain.Basics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.SetupExtensions
{
    public static class HtmlTemplateDictInitExtension
    {
        
        public static ConcurrentDictionary<string, BaseFieldType> fielddict
            = new ConcurrentDictionary<string, BaseFieldType>();

        public static void HtmlTemplateDictInit(this IServiceCollection services)
        {
            Assembly assembly = Assembly.Load("Flex.Domain");
            var fields = assembly.GetTypes().Where(m => typeof(BaseFieldType).IsAssignableFrom(m));
            foreach (var field in fields)
            {
                var activeobject = Activator.CreateInstance(field) as BaseFieldType;
                if (activeobject == null)
                    continue;
                fielddict.AddOrUpdate(field.Name, activeobject, (a, b) => activeobject);
            }
        }
    }
}
