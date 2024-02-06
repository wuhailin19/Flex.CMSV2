
namespace Flex.Domain.AutoMapper
{
    public class ColumnProfile : Profile
    {
        public ColumnProfile()
        {
            CreateMap<SysColumn, TreeColumnListDto>()
                .ForMember(a =>a.title, opt => opt.MapFrom(b => b.Name))
                .ForMember(a =>a.href, opt => opt.MapFrom(b =>b.ModelId!=0? "/system/ColumnContent/Index/" + b.Id:"javascript:;"));
            CreateMap<SysColumn, ColumnListDto>();
            CreateMap<SysColumn, UpdateColumnDto>();
            CreateMap<UpdateColumnDto, SysColumn>();
            CreateMap<AddColumnDto, SysColumn>();
        }
    }
}
