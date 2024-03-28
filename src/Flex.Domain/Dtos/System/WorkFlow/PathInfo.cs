using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.WorkFlow
{
    public class PathInfo
    {
        public Dictionary<string, StepObject> Paths { get; set; }
    }
    public class StepObject
    {
        public string AvoidFlag { get; set; }
        public string OrgMode { get; set; }
        public string StepOrg { get; set; }
        public string StepRole { get; set; }
        public string StepMan { get; set; }
        public string StepId { get; set; }
        public string FlowId { get; set; }
    }
}
