using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.ColumnContent
{
    public class FieldContentAndColumnDto
    {
        public SysContentModel ContentModel { get; set; }
        public SysColumn Column { get; set; }
        public List<sysField> Field { get; set; }
    }
}
