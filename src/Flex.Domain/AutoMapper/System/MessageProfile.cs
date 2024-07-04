using Flex.Domain.Collections;
using Flex.Domain.Dtos.Message;
using Flex.Domain.Dtos.System.Message;

namespace Flex.Domain.AutoMapper
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<sysMessage, MessageTitleListDto>();
            CreateMap<sysMessage, MessageOutputDto>();
            CreateMap<sysMessage, ApprovalProcessDto>().ForMember(m => m.MessageResult, t => t.MapFrom(m => m.MessageCate.GetEnumDescription()));
            CreateMap<SendReviewMessageDto, sysMessage>();
            CreateMap<PagedList<sysMessage>, PagedList<MessageTitleListDto>>();
        }
    }
}
