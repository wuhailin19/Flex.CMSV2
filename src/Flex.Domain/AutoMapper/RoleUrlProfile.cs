using Flex.Domain.Dtos.RoleUrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.AutoMapper
{
    public class RoleUrlProfile: Profile
    {
        public RoleUrlProfile() {
            CreateMap<SysRoleUrl, RoleUrlListDto>();
        }
    }
}
