using Flex.Application.Authorize;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Flex.Application.Extensions;
using Flex.Core.Timing;
using Flex.Core.Extensions;

namespace Flex.Application.Handlers
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _Context;
        private readonly IClaimsAccessor _claims;
        private readonly IRoleServices _roleServices;
        public PermissionHandler(IHttpContextAccessor Context, IClaimsAccessor claims, IRoleServices roleServices)
        {
            _Context = Context;
            _claims = claims;
            _roleServices = roleServices;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            HttpContext httpContext = _Context.HttpContext;
            if (context.User.Identity.IsAuthenticated)
            {
                var authHeader = httpContext.Request.Headers["Authorization"].ToString();
                if (authHeader != null && authHeader.StartsWith(JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                {
                    //Console.WriteLine("过期时间：" + DateTime.Parse(context.User.Claims.First(m => m.Type == ClaimTypes.Expiration).Value));
                    var expirationtime = DateTime.Parse(context.User.Claims.First(m => m.Type == ClaimTypes.Expiration)?.Value);
                    if (expirationtime < Clock.Now)
                    {
                        Fail(context, httpContext, StatusCodes.Status419AuthenticationTimeout);
                        return;
                    }
                    if (_claims.IsSystem)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                    var userrole = _claims.UserRole;
                    if (userrole.IsNullOrEmpty())
                    {
                        Fail(context, httpContext);

                        return;
                    }

                    //所有角色对应的接口权限
                    var RoleList = new Dictionary<string, List<string>>();
                    var role_items = userrole.Split(',');
                    RoleList = await GetRoleUrlDictByRedisOrDataServer(RoleList, role_items);
                    //当前接口链接
                    var nowurl = httpContext.Request.Path.ToString().ToLower();
                    var result = false;
                    foreach (var role in role_items)
                    {
                        if (!RoleList.ContainsKey(role))
                            continue;
                        if (RoleList[role].Contains(nowurl))
                        {
                            result = true; break;
                        }
                    }
                    if (result)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        Fail(context, httpContext);
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 获取角色和权限Url对应关系的字典
        /// </summary>
        /// <param name="RoleList"></param>
        /// <param name="role_items"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, List<string>>> GetRoleUrlDictByRedisOrDataServer(Dictionary<string, List<string>> RoleList, string[] role_items)
        {
            //if (RedisHelperFull.RedisConfig.Useable)
            //{
            //    if (await RedisHelperFull.Instance.KeyExistsAsync(RedisKeyRepository.RoleRedisKey))
            //    {
            //        var redis_result = await RedisHelperFull.Instance.HashGetAsync(RedisKeyRepository.RoleRedisKey, role_items.ToList());
            //        for (int i = 0; i < role_items.Length; i++)
            //        {
            //            RoleList.Add(role_items[i], JsonExtension.GetDeserializeObject<List<string>>(redis_result[i].ToString()));
            //        }
            //    }
            //    else
            //    {
            //        RoleList = await _roleServices.PermissionDtosAsync();
            //        var roleitems = new List<HashEntry>();
            //        foreach (var item in RoleList)
            //        {
            //            roleitems.Add(new HashEntry(item.Key, JsonHelper.ToJson(item.Value)));
            //        }
            //        await RedisHelperFull.Instance.HashSetAsync(RedisKeyRepository.RoleRedisKey, roleitems);
            //    }
            //}
            //else
            //{
            //    RoleList = await _roleServices.PermissionDtosAsync();
            //    var roleitems = new List<HashEntry>();
            //    foreach (var item in RoleList)
            //    {
            //        roleitems.Add(new HashEntry(item.Key, JsonHelper.ToJson(item.Value)));
            //    }
            //}

            return RoleList;
        }

        private void Fail(AuthorizationHandlerContext context, HttpContext httpContext, int statusCodes = StatusCodes.Status401Unauthorized)
        {
            httpContext.Response.StatusCode = statusCodes;
            context.Fail();
        }
    }
}
