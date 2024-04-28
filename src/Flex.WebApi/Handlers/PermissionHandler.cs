using Flex.Application.Authorize;
using Flex.Application.Contracts.Exceptions;
using Flex.Core;
using Flex.Core.Helper;
using Flex.Core.Helper.MemoryCacheHelper;
using Flex.Core.Timing;
using Flex.Domain.Cache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ShardingCore.Extensions;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace Flex.WebApi.Handlers
{

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _Context;
        private readonly IClaimsAccessor _claims;
        private readonly IRoleServices _roleServices;
        private readonly ILogger<PermissionHandler> _logger;
        private ICaching _caching;
        public PermissionHandler(IHttpContextAccessor Context, IClaimsAccessor claims, IRoleServices roleServices, ILogger<PermissionHandler> logger, ICaching caching)
        {
            _Context = Context;
            _claims = claims;
            _roleServices = roleServices;
            _logger = logger;
            _caching = caching;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            HttpContext httpContext = _Context.HttpContext;
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                //当前接口链接
                var nowurl = httpContext.Request.Path.ToString().ToLower();
                var authHeader = httpContext.Request.Headers["Authorization"].ToString();
                if (authHeader != null && authHeader.StartsWith(JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                {
                    //Console.WriteLine("过期时间：" + DateTime.Parse(context.User.Claims.First(m => m.Type == ClaimTypes.Expiration).Value));
                    var expirationtime = DateTime.Parse(context.User.Claims.First(m => m.Type == ClaimTypes.Expiration)?.Value);
                    if (expirationtime < Clock.Now)
                    {
                        _logger.LogWarning("该用户{0}（{2}），登录已超时，链接{1}", _claims.UserName, nowurl, _claims.UserId);
                        Fail(context, httpContext, StatusCodes.Status419AuthenticationTimeout);
                        return;
                    }
                    if (_claims.IsSystem)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                    var userrole = _claims.UserRole;
                    if (userrole == 0)
                    {
                        _logger.LogWarning("该用户{0}（{2}），没有角色信息，链接{1}", _claims.UserName, nowurl, _claims.UserId);
                        Fail(context, httpContext);
                        return;
                    }
                    //所有角色对应的接口权限
                    var RoleList = await GetRoleUrlDictByRedisOrDataServer();
                    var checkurl = nowurl.TrimEnd('/') + "/";
                    var result = false;
                    if (RoleList.ContainsKey(userrole))
                    {
                        foreach (var item in RoleList[userrole])
                        {
                            if (checkurl.Contains(item.TrimEnd('/') + "/"))
                            {
                                result = true; break;
                            }
                        }
                    }
                    if (result)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        _logger.LogWarning("该用户{0}（{1}），没有权限访问，链接{2}", _claims.UserName, _claims.UserId, nowurl);
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
        private async Task<Dictionary<int, List<string>>> GetRoleUrlDictByRedisOrDataServer()
        {
            #region Redis缓存版本
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
            #endregion
            var userid = RoleKeys.userRoleKey + _claims.UserRole;
            Dictionary<int, List<string>> RoleList;
            if (_caching.Get(userid) == null)
            {
                RoleList = await _roleServices.CurrentUrlPermissionDtosAsync();
                _caching.Set(userid, RoleList, new TimeSpan(1, 0, 0));
            }
            else
            {
                RoleList = _caching.Get(userid) as Dictionary<int, List<string>>;
            }
            return RoleList;
        }


        private void Fail(AuthorizationHandlerContext context, HttpContext httpContext, int statusCodes = StatusCodes.Status401Unauthorized)
        {
            httpContext.Response.StatusCode = statusCodes;
            context.Fail();
        }
    }
}
