using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Cache
{
    public class RoleKeys
    {
        /// <summary>
        /// 角色可访问的接口缓存键
        /// </summary>
        public const string userRoleKey = "userRoleUrlKey";
        /// <summary>
        /// 角色可访问的栏目数据权限缓存键
        /// </summary>
        public const string userDataPermissionKey = "userDataPermissionKey";
    }
}
