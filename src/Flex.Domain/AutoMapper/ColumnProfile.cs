
namespace Flex.Domain.AutoMapper
{
    public class ColumnProfile : Profile
    {
        public ColumnProfile()
        {
            CreateMap<SysColumn, TreeColumnListDto>();
        }
    }
}
