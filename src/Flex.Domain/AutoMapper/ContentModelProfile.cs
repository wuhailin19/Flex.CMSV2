using Flex.Domain.Dtos.ContentModel;

namespace Flex.Domain.AutoMapper
{
    public class ContentModelProfile : Profile
    {
        public ContentModelProfile()
        {
            CreateMap<SysContentModel, ContentModelColumnDto>();
            CreateMap<AddContentModelDto, SysContentModel>();
        }
    }
}
