using Flex.Domain.Collections;
using Flex.Domain.Dtos.RoleUrl;
using Flex.Domain.Dtos.System.TableRelation;
using Flex.Domain.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.AutoMapper
{
    public class TableRelationProfile: Profile
    {
        public TableRelationProfile() {
            CreateMap<sysTableRelation, TableRelationColumnDto>();
            CreateMap<sysTableRelation, TableRelationListDto>();
            CreateMap<sysTableRelation, TableRelationRecursionDto>();
            CreateMap<AddTableRelationDto, sysTableRelation>();
            CreateMap<UpdateTableRelationDto, sysTableRelation>();

            CreateMap<PagedList<sysTableRelation>, PagedList<TableRelationColumnDto>>();
        }
    }
}
