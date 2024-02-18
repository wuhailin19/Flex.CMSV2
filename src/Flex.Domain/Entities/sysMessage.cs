using Flex.Core.JsonConvertExtension;
using Flex.Domain.Enums.Message;
using Newtonsoft.Json;

namespace Flex.Domain.Entities
{
    public class sysMessage : BaseIntEntity, EntityContext
    {
        /// <summary>
        /// 消息队列堆栈名
        /// </summary>
        public string? RabbitMqQueueName { set; get; }
        [JsonConverter(typeof(IdToStringConverter))]
        public long ToUserId { set; get; }
        public int ToRoleId { set; get; }
        public bool IsRead { set; get; } = false;
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageCate MessageCate { set; get; } = MessageCate.Nomal;
    }
}
