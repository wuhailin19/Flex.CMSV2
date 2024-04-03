using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.WorkFlow
{
    public class PathInfoExtended
    {
        public Dictionary<string, ActionObject> Paths { get; set; }
    }
    public class ActionObject
    {
        public string DirectMode { get; set; }
        public string ConjunctManFlag { get; set; }
        public string OrgBossMode { get; set; }
        public string ActionId { get; set; }
    }
}
