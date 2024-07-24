using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.ISignalRBus.Enum
{
    public enum GlobalTaskStatus
    {
        [Description("等待中")]
        Waiting,
        [Description("运行中")]
        Running,
        [Description("已结束")]
        Ending
    }
}
