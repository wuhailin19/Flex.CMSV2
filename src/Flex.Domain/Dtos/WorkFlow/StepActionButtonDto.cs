using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.WorkFlow
{
    public class StepActionButtonDto
    {
        public string Id { set; get; }
        public string actionPathId { set; get; }
        public string actionName { set; get; }
        public string stepFromId { set; get; }
        public string stepToId { set; get; }
        public string actionFromName { set; get; }
        public string actionToName { set; get; }
    }
}
