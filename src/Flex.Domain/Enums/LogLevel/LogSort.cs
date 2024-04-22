using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Enums.LogLevel
{
    public enum LogSort
    {
        [Description("登录")]
        Login,
        [Description("接口日志")]
        Api,
        [Description("内容日志")]
        DataOperation
    }
}
