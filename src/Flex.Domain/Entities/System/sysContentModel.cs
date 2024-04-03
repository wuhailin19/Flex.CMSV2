using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities
{
    public class SysContentModel : BaseIntEntity, EntityContext
    {
        public string Name { set; get; }
        public string? Description { set; get; }
        public string TableName { set; get; }
        public string? FormHtmlString { set; get; }
    }
}
