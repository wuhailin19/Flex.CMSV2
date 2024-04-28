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
        Login = 0,
        [Description("接口日志")]
        Api = 1,
        [Description("内容日志")]
        DataOperation = 2
    }
}
