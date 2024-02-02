using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.WorkFlow
{
    public class InputWorkFlowContentDto
    {
        public int Id { get; set; }
        public string design { get; set; }
        public string stepDesign { get; set; }
        public string actDesign { get; set; }
    }
}
