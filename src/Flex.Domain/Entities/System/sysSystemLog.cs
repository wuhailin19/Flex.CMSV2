using Flex.Domain.Enums.LogLevel;

namespace Flex.Domain.Entities.System
{
    public class sysSystemLog : BaseLongEntity, EntityContext
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 日志等级
        /// </summary>
        public SystemLogLevel LogLevel { get; set; }
        /// <summary>
        /// 日志分类
        /// </summary>
        public LogSort LogSort { get; set; }
    }
}
