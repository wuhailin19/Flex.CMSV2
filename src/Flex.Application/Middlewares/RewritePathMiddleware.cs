using Flex.Domain.Cache;
using Flex.Domain.Dtos.System.SiteManage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Middlewares
{
    public class RewritePathMiddleware
    {
        private readonly RequestDelegate _next;
        private ISiteManageServices _siteManageServices;
        private ICaching _caching;
        public RewritePathMiddleware(RequestDelegate next, ISiteManageServices siteManageServices, ICaching caching)
        {
            _next = next;
            _siteManageServices = siteManageServices;
            _caching = caching;
        }

        public async Task Invoke(HttpContext context)
        {
            List<SiteManageColumnDto> list = new List<SiteManageColumnDto>();
            if (_caching.Exist(SiteKeys.SiteRouteKeys))
            {
                list = _caching.Get(SiteKeys.SiteRouteKeys) as List<SiteManageColumnDto>;
            }
            else
            {
                list = (await _siteManageServices.ListAsync()).ToList();
                _caching.Set(SiteKeys.SiteRouteKeys, list, new TimeSpan(24, 0, 0));
            }

            // 检查请求路径是否以 "/en/" 开头
            foreach (var item in list)
            {
                if (item.RoutePrefix.IsNullOrEmpty())
                    continue;
                var customroute = "/" + item.RoutePrefix.TrimStart('/');
                var targetroute = "/" + item.TargetRoutePrefix.TrimStart('/');
                if (context.Request.Path.ToString().StartsWith(customroute))
                {
                    // 替换路径中的 "/en/" 为 "/111/"
                    context.Request.Path = context.Request.Path.Value.Replace(customroute, targetroute);
                }
            }
            // 将请求传递给下一个中间件或处理程序
            await _next(context);
        }
    }

    public static class RewritePathMiddlewareExtensions
    {
        public static IApplicationBuilder UseRewritePathMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RewritePathMiddleware>();
        }
    }
}
