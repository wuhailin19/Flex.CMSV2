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
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        Approved = 1,
        /// <summary>
        /// 退稿
        /// </summary>
        [Description("退稿")]
        Rejected = 0,
        /// <summary>
        /// 普通流程
        /// </summary>
        [Description("普通流程")]
        NormalTask = 2
    }
}
