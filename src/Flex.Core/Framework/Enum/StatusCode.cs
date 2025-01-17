﻿using Flex.Core.Attributes;
using System.ComponentModel;

namespace Flex.Core
{
    public enum StatusCode
    {
        [Description("已删除")]
        [Color(Color ="red")]
        Deleted = -1,//软删除，已删除的无法恢复，无法看见，暂未使用
        [Description("正常")]
        [Color(Color = "green")]
        Enable = 1,
        [Description("失效")]
        [Color(Color = "red")]
        Disable = 0,//失效的还可以改为生效
        [Description("草稿")]
        [Color(Color = "red")]
        Draft = 2,
        [Description("待发布")]
        [Color(Color = "orange")]
        PendingRelease = 4,
        [Color(Color = "lightsalmon")]
        [Description("待审核")]
        PendingApproval = 5,
        [Color(Color = "lightsalmon")]
        [Description("历史版本")]
        HistoryVersion = 6,

    }
    
}
