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
        /// 审核开始
        /// </summary>
        [Description("审核开始")]
        Begin = 0,
        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        Approved = 1,
        /// <summary>
        /// 退稿
        /// </summary>
        [Description("退稿")]
        Rejected = 2,
        /// <summary>
        /// 普通流程
        /// </summary>
        [Description("普通流程")]
        NormalTask = 3,
        /// <summary>
        /// 否决
        /// </summary>
        [Description("否决")]
        Veto = 4
    }
}
