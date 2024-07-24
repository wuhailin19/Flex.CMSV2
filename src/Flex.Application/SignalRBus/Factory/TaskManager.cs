using Flex.Domain.Basics;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace Flex.Application.SignalRBus.Factory
{
    public static class TaskManager
    {
        public static ConcurrentDictionary<string, RequestModel> requestDict
        = new ConcurrentDictionary<string, RequestModel>();

        public static void InitRequesModelDict(this IServiceCollection services)
        {
            Assembly assembly = Assembly.Load("Flex.Domain");
            var fields = assembly.GetTypes().Where(m => typeof(RequestModel).IsAssignableFrom(m));
            foreach (var field in fields)
            {
                var activeobject = Activator.CreateInstance(field) as RequestModel;
                if (activeobject == null)
                    continue;
                requestDict.AddOrUpdate(field.Name, activeobject, (a, b) => activeobject);
            }
        }
    }
}
