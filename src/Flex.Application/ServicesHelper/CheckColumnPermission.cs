using Flex.Core.Helper.MemoryCacheHelper;
using Flex.Domain.Cache;
using Flex.Domain.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.ServicesHelper
{
    public class CheckColumnPermission
    {
        /// <summary>
        /// 判断当前角色有无栏目权限
        /// </summary>
        /// <param name="currentdto"></param>
        /// <param name="updatedto"></param>
        /// <returns></returns>
        public static bool CheckPermission(DataPermissionDto currentdto, DataPermissionDto updatedto)
        {
            if (currentdto == null)
                return false;

            var splist = updatedto.sp.ToList("-");
            var dplist = updatedto.dp.ToList("-");
            var adlist = updatedto.ad.ToList("-");
            var edlist = updatedto.ed.ToList("-");

            Dictionary<string, List<string>> filedvalue = new Dictionary<string, List<string>>
                {
                    { nameof(DataPermissionDto.sp), currentdto.sp.ToList("-") },
                    { nameof(DataPermissionDto.ed), currentdto.ed.ToList("-") },
                    { nameof(DataPermissionDto.ad), currentdto.ad.ToList("-") },
                    { nameof(DataPermissionDto.dp),currentdto.dp.ToList("-") }
                };
            foreach (var item in splist)
            {
                var result =  filedvalue[nameof(DataPermissionDto.sp)].Contains(item);
                if (!result)
                    return false;
            }
            foreach (var item in dplist)
            {
                var result = filedvalue[nameof(DataPermissionDto.ed)].Contains(item);
                if (!result)
                    return false;
            }
            foreach (var item in adlist)
            {
                var result = filedvalue[nameof(DataPermissionDto.ad)].Contains(item);
                if (!result)
                    return false;
            }
            foreach (var item in edlist)
            {
                var result = filedvalue[nameof(DataPermissionDto.dp)].Contains(item);
                if (!result)
                    return false;
            }
            return true;
        }
    }
}
