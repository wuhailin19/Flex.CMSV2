using Flex.Domain.Enums.LogLevel;

namespace Flex.Domain.Dtos.System.SystemLog
{
    public class InputSystemLogDto
    {
        public string OperationContent { get; set; }
        public string Url { get; set; }
        public SystemLogLevel LogLevel { get; set; } = SystemLogLevel.Normal;
        /// <summary>
        /// 日志分类
        /// </summary>
        public LogSort LogSort { get; set; }
    }
}
