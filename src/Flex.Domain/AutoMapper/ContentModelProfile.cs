using Flex.Domain.Dtos.ContentModel;

namespace Flex.Domain.AutoMapper
{
    public class ContentModelProfile : Profile
    {
        public ContentModelProfile()
        {
            CreateMap<SysContentModel, ContentModelColumnDto>();
            CreateMap<SysContentModel, ContentSelectItemDto>();
            CreateMap<AddContentModelDto, SysContentModel>()
                .ForMember(a => a.TableName, b => b.MapFrom(c => 
                "tbl_normal_" + c.TableName
                .Replace("tbl_normal_", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase)));
        }
    }
}
