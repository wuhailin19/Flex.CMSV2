using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Extensions.SetupExtensions
{
    public static class SwaggerSetupExtension
    {
        public static void AddSwaggerSetupExtension(this IServiceCollection services)
        {
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml" ?? string.Empty;
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Flex.Cms", new OpenApiInfo { Title = "Flex.Cms API", Version = "v1" });
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
