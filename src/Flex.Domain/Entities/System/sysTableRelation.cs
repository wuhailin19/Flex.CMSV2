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
        /// <summary>
        /// 是否只用于本站点
        /// </summary>
        public bool SelfUse { get; set; }
        public int SiteId { get; set; }
        public string LinkName { get; set; }
    }
}
