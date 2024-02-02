using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities
{
    public class sysWorkFlowAction : BaseEntity, EntityContext
    {
        public string actionPathId { set; get; }
        public string actionName { set; get; }
        public int flowId { set; get; }
        public string? conjunctManFlag { set; get; }
        public string? directMode { set; get; }
        public string? orgBossMode { set; get; }
        public string? stepFromId { set; get; }
        public string? stepToId { set; get; }
        public string? actionFromName { set; get; }
        public string? actionToName { set; get; }
    }
}
