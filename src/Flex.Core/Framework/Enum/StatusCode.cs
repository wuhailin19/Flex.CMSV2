using System.ComponentModel;

namespace Flex.Core
{
    public enum StatusCode
    {
        [Description("已删除")]
        Deleted = -1,//软删除，已删除的无法恢复，无法看见，暂未使用
        [Description("生效")]
        Enable = 1,
        [Description("失效")]
        Disable = 0,//失效的还可以改为生效
        [Description("草稿")]
        Draft = 2,
        [Description("待发布")]
        PendingRelease = 4,
        [Description("待审核")]
        PendingApproval = 4,

    }
    
}
