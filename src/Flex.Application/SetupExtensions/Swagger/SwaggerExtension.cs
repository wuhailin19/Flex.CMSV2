using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Flex.Application.Extensions.Swagger
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services, string ApiName)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    // {ApiName} 定义成全局变量，方便修改
                    Version = "v1",
                    Title = $"{ApiName} 接口文档——Netcore 6.0",
                    Description = $"{ApiName} HTTP API v1"
                });
                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // Jwt Bearer 认证，必须是 oauth2
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                c.OrderActionsBy(o => o.RelativePath);
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    c.IncludeXmlComments(file, true);
                });
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{ApiName}.xml"), true);
            });
        }
    }
}
