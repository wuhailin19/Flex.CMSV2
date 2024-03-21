using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Flex.Core.Helper.LogHelper
{
    public class StaticLoggerFactory
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddNLog());
    }
}
