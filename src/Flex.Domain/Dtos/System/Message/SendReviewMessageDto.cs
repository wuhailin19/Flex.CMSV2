using Flex.Domain.Enums.Message;
using System.Collections;

namespace Flex.Domain.Dtos.Message
{
    public class SendReviewMessageDto
    {
        public string Title { set; get; }
        public string? MsgContent { set; get; }
        public string ToPathId { set; get; }
        public string FromPathId { set; get; }
        public int ParentId { set; get; }
        public int ModelId { set; get; }
        public int ContentId { set; get; }
        public Hashtable BaseFormContent { set; get; }
        public MessageCate MessageCate { set; get; } = MessageCate.NormalTask;
        /// <summary>
        /// 回复ID
        /// </summary>
        public int? RecieveId { set; get; }
    }
}
