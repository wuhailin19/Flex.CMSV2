using AutoMapper;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Entities.System;
using Flex.Domain.Collections;

namespace Flex.Domain.AutoMapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<SysRole, RoleDto>();
            CreateMap<SysRole, RoleColumnDto>();
            CreateMap<PagedList<SysRole>, PagedList<RoleColumnDto>>();
        }
    }
}
