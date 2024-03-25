using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities
{
    public class sysWorkFlow : BaseIntEntity, EntityContext
    {
        public string Name { get; set; }
        public string? WorkFlowContent { get; set; }
        public string? stepDesign { get; set; }
        public string? actDesign { get; set; }
        public string? actionString { get; set; }
        public string? Introduction { get; set; }
    }
}
