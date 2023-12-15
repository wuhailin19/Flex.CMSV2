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
            CreateMap<SimpleAdminDto, SysAdmin>();
            CreateMap<SimpleEditAdminDto, SysAdmin>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.UserAvatar))
            .ForMember(dest => dest.UserSign, opt => opt.MapFrom(src => src.UserSign))
            .ForMember(dest => dest.FilterIp, opt => opt.MapFrom(src => src.FilterIp))
            .ForMember(dest => dest.AllowMultiLogin, opt => opt.MapFrom(src => src.AllowMultiLogin))
            .ForMember(dest => dest.Islock, opt => opt.MapFrom(src => src.Islock));
            CreateMap<PagedList<SysAdmin>, PagedList<AdminDto>>();
        }
    }
}
