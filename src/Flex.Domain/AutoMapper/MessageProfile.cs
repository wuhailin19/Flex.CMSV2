using Flex.Domain.Collections;
using Flex.Domain.Dtos.Message;

namespace Flex.Domain.AutoMapper
{
    public class MessageProfile : Profile
    {
        public MessageProfile() {
            CreateMap<sysMessage, MessageTitleListDto>();
            CreateMap<sysMessage, MessageOutputDto>();
            CreateMap<SendReviewMessageDto, sysMessage>();
            CreateMap<PagedList<sysMessage>, PagedList<MessageTitleListDto>>();
        }
    }
}
