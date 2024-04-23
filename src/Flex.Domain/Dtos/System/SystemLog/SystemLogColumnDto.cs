using Microsoft.Extensions.Logging;

namespace Flex.Domain.Dtos.System.SystemLog
{
    public class SystemLogColumnDto
    {
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center)]
        public long Id { set; get; }
        /// <summary>
        /// 操作人
        /// </summary>
        [ToolAttr(NameAttr = "操作人", AlignAttr = AlignEnum.Center)]
        public string Operator { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [ToolAttr(NameAttr = "操作内容", Toolbar = "#DescInfo")]
        public string OperationContent { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        [ToolAttr(NameAttr = "角色名", AlignAttr = AlignEnum.Center)]
        public string RoleName { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        [ToolAttr(NameAttr = "IP地址", AlignAttr = AlignEnum.Center)]
        public string Ip { get; set; }
        /// <summary>
        /// Url地址
        /// </summary>
        [ToolAttr(NameAttr = "访问地址")]
        public string Url { get; set; }
        /// <summary>
        /// 日志等级
        /// </summary>
        [ToolAttr(NameAttr = "日志等级", AlignAttr = AlignEnum.Center, Toolbar = "#LogStatus")]
        public string LogLevel { get; set; }

        [ToolAttr(NameAttr = "日志分类", HideFiled = true)]
        public string LogSort { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [ToolAttr(NameAttr = "操作时间")]
        public DateTime AddTime { get; set; }
    }
}
