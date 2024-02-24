using Flex.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Flex.Application.Authorize
{
    public class ClaimsAccessor : IClaimsAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal UserPrincipal
        {
            get
            {
                ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
                if (user.Identity?.IsAuthenticated ?? false)
                {
                    return user;
                }
                else
                {
                    throw new Exception("用户未认证");
                }
            }
        }
        public string UserName
        {
            get
            {
                return UserPrincipal.Claims.First(x => x.Type == UserClaimType.Name).Value;
            }
        }
        public long UserId
        {
            get
            {
                return long.Parse(UserPrincipal.Claims.First(x => x.Type == UserClaimType.Id).Value);
            }

        }
        public string UserAccount
        {
            get
            {
                return UserPrincipal.Claims.First(x => x.Type == UserClaimType.Account).Value;
            }
        }
        public int UserRole
        {
            get
            {
                return UserPrincipal.Claims.First(x => x.Type == UserClaimType.RoleId).Value.ToInt();
            }
        }
        /// <summary>
        /// 判断是不是系统管理员
        /// </summary>
        public bool IsSystem { get { return UserRole == 0; } }
        public string UserRoleDisplayName
        {
            get
            {
                return UserPrincipal.Claims.First(x => x.Type == UserClaimType.RoleDisplayName).Value;
            }
        }
    }
}
