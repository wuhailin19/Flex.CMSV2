using Flex.Core.JsonConvertExtension;
using Flex.Domain.Enums.Message;
using Newtonsoft.Json;

namespace Flex.Domain.Entities
{
    public class sysMessage : BaseIntEntity, EntityContext
    {
        public string Title { set; get; }
        public string? MsgContent { set; get; }
        /// <summary>
        /// 消息队列堆栈名
        /// </summary>
        public string? RabbitMqQueueName { set; get; }
        public string? ToUserId { set; get; }
        public string? ToRoleId { set; get; }
        public bool IsRead { set; get; } = false;
        /// <summary>
        /// 回复ID
        /// </summary>
        public int RecieveId { set; get; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageCate MessageCate { set; get; } = MessageCate.Nomal;
    }
}
