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
            CreateMap<SysRole, RoleSelectDto>();
            CreateMap<InputRoleDto, SysRole>()
                .ForMember(dest => dest.RolesName, opt => opt.MapFrom(src => src.RolesName))
                .ForMember(dest => dest.RolesDesc, opt => opt.MapFrom(src => src.RolesDesc));
            CreateMap<InputUpdateRoleDto, SysRole>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RolesName, opt => opt.MapFrom(src => src.RolesName))
                .ForMember(dest => dest.RolesDesc, opt => opt.MapFrom(src => src.RolesDesc));
            CreateMap<InputRoleMenuDto, SysRole>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MenuPermissions, opt => opt.MapFrom(src => src.MenuPermissions));
            CreateMap<PagedList<SysRole>, PagedList<RoleColumnDto>>();
        }
    }
}
