using Flex.Core.JsonConvertExtension;
using Flex.Domain.Enums.Message;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using SqlSugar;
namespace Flex.Domain.Entities
{
    [Table(name: "tbl_core_message")]
    [SugarTable("tbl_core_message")]
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
        public string? FromPathId { set; get; }
        public string? ToPathId { set; get; }
        public bool IsRead { set; get; } = false;
        public string? TableName { set; get; }
        public int ContentId { set; get; }
        public int ParentId { set; get; }
        public int ModelId { set; get; }
        public int FlowId { set; get; }
        public bool IsStart { set; get; } = false;
        public bool IsEnd { set; get; } = false;

        /// <summary>
        /// 消息分组Id，用于区分不同工作流
        /// </summary>
        [JsonConverter(typeof(IdToStringConverter))]
        public long MsgGroupId { set; get; }
        /// <summary>
        /// 回复ID
        /// </summary>
        public int RecieveId { set; get; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageCate MessageCate { set; get; } = MessageCate.NormalTask;
    }
}
