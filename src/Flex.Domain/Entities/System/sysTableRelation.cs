using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities.System
{
    public class sysTableRelation : BaseIntEntity, EntityContext
    {
        public int ParentModelId { get; set; }
        //public string ChildLinkurl { get; set; }
        public int ChildModelId { get; set; }
        public string LinkName { get; set; }
    }
}
