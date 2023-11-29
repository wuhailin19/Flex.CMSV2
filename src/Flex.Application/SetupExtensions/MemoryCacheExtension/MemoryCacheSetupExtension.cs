using Flex.Core.Helper.MemoryCacheHelper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Flex.Application.Extensions.Register.MemoryCacheExtension
{
    public static class MemoryCacheSetupExtension
    {
        public static void AddMemoryCacheSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<ICaching, MemoryCaching>();
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var value = factory.GetRequiredService<IOptions<MemoryCacheOptions>>();
                var cache = new MemoryCache(value);
                return cache;
            });
        }
    }
}
