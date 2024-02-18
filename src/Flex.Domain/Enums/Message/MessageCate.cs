using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Enums.Message
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageCate
    {
        /// <summary>
        /// 审核类
        /// </summary>
        [Description("审核消息")]
        Review = 1,
        /// <summary>
        /// 普通消息
        /// </summary>
        [Description("普通消息")]
        Nomal = 2
    }
}
