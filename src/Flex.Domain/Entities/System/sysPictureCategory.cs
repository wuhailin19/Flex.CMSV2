using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities
{
    public class sysPictureCategory : BaseIntEntity, EntityContext
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public int OrderId { set; get; }
    }
}
