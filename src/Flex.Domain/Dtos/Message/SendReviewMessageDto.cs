using Flex.Core.JsonConvertExtension;
using Flex.Domain.Enums.Message;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Message
{
    public class SendReviewMessageDto
    {
        public string Title { set; get; }
        public string? MsgContent { set; get; }
        public string stepId { set; get; }
        public string TableName { set; get; }
        public int ContentId { set; get; }
        public MessageCate MessageCate { set; get; } = MessageCate.Nomal;
        /// <summary>
        /// 回复ID
        /// </summary>
        public int? RecieveId { set; get; }
    }
}
