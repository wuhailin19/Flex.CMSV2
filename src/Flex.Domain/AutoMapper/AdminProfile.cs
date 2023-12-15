using AutoMapper;
using Flex.Domain.Dtos;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Entities.System;
using Flex.Domain.Collections;

namespace Flex.Domain.AutoMapper
{
    public sealed class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<SysAdmin, AdminDto>();
            CreateMap<SysAdmin, UserData>();
            CreateMap<SysAdmin, SimpleAdminDto>();
            CreateMap<PagedList<SysAdmin>, PagedList<AdminDto>>();
        }
    }
}
