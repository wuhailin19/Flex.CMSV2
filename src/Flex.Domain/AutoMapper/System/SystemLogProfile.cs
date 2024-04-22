using Flex.Domain.Collections;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Entities.System;

namespace Flex.Domain.AutoMapper.System
{
    public class SystemLogProfile : Profile
    {
        public SystemLogProfile()
        {
            CreateMap<sysSystemLog, SystemLogColumnDto>()
                .ForMember(m => m.LogLevel, n => n.MapFrom(c => c.LogLevel.GetEnumDescription()))
                .ForMember(m => m.LogSort, n => n.MapFrom(c => c.LogSort.GetEnumDescription()));

            CreateMap<PagedList<sysSystemLog>, PagedList<SystemLogColumnDto>>();
        }
    }
}
