using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Enums.LogLevel
{
    /// <summary>
    /// 系统操作日志记录等级
    /// </summary>
    public enum SystemLogLevel
    {
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error = -1,
        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning = -2,
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login = 1,
        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通")]
        Normal = 2
    }
}
