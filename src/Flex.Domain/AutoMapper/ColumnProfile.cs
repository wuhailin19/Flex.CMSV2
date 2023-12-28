
namespace Flex.Domain.AutoMapper
{
    public class ColumnProfile : Profile
    {
        public ColumnProfile()
        {
            CreateMap<SysColumn, TreeColumnListDto>()
                .ForMember(a =>a.title, opt => opt.MapFrom(b => b.Name));
            CreateMap<SysColumn, ColumnListDto>();
            CreateMap<SysColumn, UpdateColumnDto>();
            CreateMap<AddColumnDto, SysColumn>();
        }
    }
}
