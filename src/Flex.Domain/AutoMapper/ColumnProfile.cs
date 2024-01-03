
namespace Flex.Domain.AutoMapper
{
    public class ColumnProfile : Profile
    {
        public ColumnProfile()
        {
            CreateMap<SysColumn, TreeColumnListDto>()
                .ForMember(a =>a.title, opt => opt.MapFrom(b => b.Name))
                .ForMember(a =>a.href, opt => opt.MapFrom(b => "/system/ColumnContent/Index/" + b.Id));
            CreateMap<SysColumn, ColumnListDto>();
            CreateMap<SysColumn, UpdateColumnDto>();
            CreateMap<AddColumnDto, SysColumn>();
        }
    }
}
