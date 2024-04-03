using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities
{
    public class sysWorkFlowStep : BaseEntity, EntityContext
    {
        public string stepPathId { set; get; }
        public string stepName { set; get; }
        public int flowId { set; get; }
        public string? avoidFlag { set; get; }
        public string? orgMode { set; get; }
        public string? stepMan { set; get; }
        public string? stepOrg { set; get; }
        public string? stepRole { set; get; }
        public string? stepCate { set; get; }
        public StepProperty isStart { set; get; }
    }
}
/// <summary>
/// 节点起始状态
/// </summary>
public enum StepProperty
{
    Start = 1,
    Other = 0
}
