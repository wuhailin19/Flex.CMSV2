using Flex.Core.Attributes;
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
        [Color(Color = "layui-btn-danger")]
        Error = -1,
        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        [Color(Color = "layui-btn-warm")]
        Warning = -2,
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        [Color(Color = "layui-btn-login")]
        Login = 1,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        [Color(Color = "layui-btn-normal")]
        Normal = 2,
        /// <summary>
        /// 全部
        /// </summary>
        All = 0
    }
}
