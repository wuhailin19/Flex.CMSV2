using Flex.Core.Helper;
using Flex.Domain.Collections;
using Flex.Domain.Dtos;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Entities.System;

namespace Flex.Domain.AutoMapper
{
    public sealed class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<SysAdmin, AdminDto>();
            CreateMap<SysAdmin, UserData>();
            CreateMap<SysAdmin, SimpleAdminDto>()
                .ForMember(dest => dest.adminLoginLog,opt=>opt.MapFrom(src=> DeserializeLoginLog(src.LoginLogString)));
            CreateMap<SimpleAdminDto, SysAdmin>();
            CreateMap<AdminAddDto, SysAdmin>();
            CreateMap<SysAdmin, AdminColumnDto>();
            
            CreateMap<SysAdmin, AdminEditInfoDto>()
                .ForMember(dest => dest.adminLoginLog, opt => opt.MapFrom(src => DeserializeLoginLog(src.LoginLogString)));

            CreateMap<SimpleEditAdminDto, SysAdmin>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.UserAvatar))
            .ForMember(dest => dest.UserSign, opt => opt.MapFrom(src => src.UserSign))
            .ForMember(dest => dest.FilterIp, opt => opt.MapFrom(src => src.FilterIp))
            .ForMember(dest => dest.AllowMultiLogin, opt => opt.MapFrom(src => src.AllowMultiLogin))
            .ForMember(dest => dest.Islock, opt => opt.MapFrom(src => src.Islock));

            CreateMap<AdminEditDto, SysAdmin>()
                        .ForMember(dest => dest.Password, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Password)))
                        .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.UserAvatar))
                        .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                        .ForMember(dest => dest.UserSign, opt => opt.MapFrom(src => src.UserSign))
                        .ForMember(dest => dest.FilterIp, opt => opt.MapFrom(src => src.FilterIp))
                        .ForMember(dest => dest.ErrorCount, opt => opt.MapFrom(src => src.ErrorCount))
                        .ForMember(dest => dest.MaxErrorCount, opt => opt.MapFrom(src => src.MaxErrorCount))
                        .ForMember(dest => dest.AllowMultiLogin, opt => opt.MapFrom(src => src.AllowMultiLogin))
                        .ForMember(dest => dest.LockTime, opt => opt.MapFrom(src => src.LockTime))
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
