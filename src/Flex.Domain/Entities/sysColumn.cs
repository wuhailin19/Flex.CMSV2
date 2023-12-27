using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities
{
    public class SysColumn : BaseIntEntity, EntityContext
    {
        public int SiteId { set; get; }
        public string Name { set; get; }
        public int ModelId { set; get; }
        public int ParentId { set; get; }
        public bool IsShow { set; get; }
        public string ColumnUrl { set; get; }
        public int OrderId { set; get; }
    }
}
