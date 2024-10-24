using Flex.Core.IDCode;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flex.Application.Extensions.Register.WebCoreExtensions
{
    public static class WebCoreSetupExtension
    {
        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// 添加跨域策略，从appsetting中读取配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder
                    .WithOrigins("Startup:Cors:AllowOrigins".Config(string.Empty).Split(","))
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
                });
            });
            return services;
        }
        /// <summary>
        /// 注册WebCore服务，配置网站
        /// do other things
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebCoreService(this IServiceCollection services)
        {
            #region 单例化雪花算法
            string workIdStr = "SiteSetting:WorkerId".Config(string.Empty);
            string datacenterIdStr = "SiteSetting:DataCenterId".Config(string.Empty); 
            long workId;
            long datacenterId;
            try
            {
                workId = workIdStr.ToLong();
                datacenterId = datacenterIdStr.ToLong();
            }
            catch (Exception)
            {
                throw;
            }
            IdWorker idWorker = new IdWorker(workId, datacenterId);
            services.AddSingleton(idWorker);

            #endregion
            return services;
        }
    }
}
