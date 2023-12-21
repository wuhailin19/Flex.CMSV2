using AutoMapper;
using Flex.Domain.Dtos;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Entities.System;
using Flex.Domain.Collections;
using Flex.Core.Helper;

namespace Flex.Domain.AutoMapper
{
    public sealed class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<SysAdmin, AdminDto>();
            CreateMap<SysAdmin, UserData>();
            CreateMap<SysAdmin, SimpleAdminDto>().ForMember(dest => dest.adminLoginLog,opt=>opt.MapFrom(src=> DeserializeLoginLog(src.LoginLogString)));
            CreateMap<SimpleAdminDto, SysAdmin>();
            CreateMap<SysAdmin, AdminColumnDto>();
            CreateMap<AdminEditDto, SysAdmin>();
            CreateMap<SimpleEditAdminDto, SysAdmin>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.UserAvatar))
            .ForMember(dest => dest.UserSign, opt => opt.MapFrom(src => src.UserSign))
            .ForMember(dest => dest.FilterIp, opt => opt.MapFrom(src => src.FilterIp))
            .ForMember(dest => dest.AllowMultiLogin, opt => opt.MapFrom(src => src.AllowMultiLogin))
            .ForMember(dest => dest.Islock, opt => opt.MapFrom(src => src.Islock));

			CreateMap<PagedList<SysAdmin>, PagedList<AdminDto>>();
            CreateMap<PagedList<SysAdmin>, PagedList<AdminColumnDto>>();
        }
        private AdminLoginLog DeserializeLoginLog(string loginLogString)
        {
            // 这里使用你的反序列化逻辑，例如使用 JsonHelper.Json<T>
            // 示例中仅为演示目的，实际实现可能会有所不同
            return JsonHelper.Json<AdminLoginLog>(loginLogString);
        }
    }
}
