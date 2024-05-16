using Microsoft.Extensions.DependencyInjection;

namespace Flex.Application.Extensions.Register.AutoMapper
{
    public static class AutoMapperSetupExtension
    {
        /// <summary>
        /// 注册automapper服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutomapperService(this IServiceCollection services)
        {
            var assemblies = DAssemblyFinder.Instance.FindAll();
            assemblies = assemblies
                .Where(item =>
                            item.ExportedTypes
                            .Where(type =>
                                        typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract
                                        ).Count() > 0);
            //将AutoMapper映射配置所在的程序集名称注册
            services.AddAutoMapper(assemblies);

            return services;
        }
    }
}
