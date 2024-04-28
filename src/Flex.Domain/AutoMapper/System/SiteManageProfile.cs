using Flex.Core.Helper;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using Flex.Domain.Dtos.System.SiteManage;
using Flex.Domain.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.AutoMapper
{
    public class SiteManageProfile : Profile
    {
        public SiteManageProfile() {
            CreateMap<sysSiteManage, SiteManageColumnDto>();
            CreateMap<AddSiteManageDto, sysSiteManage>();
            CreateMap<UpdateSiteManageDto, sysSiteManage>();
        }
    }
}
