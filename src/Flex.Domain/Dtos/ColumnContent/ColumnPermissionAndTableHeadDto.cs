using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.ColumnContent
{
    public class ColumnPermissionAndTableHeadDto
    {
        public IEnumerable<ModelTools<ColumnContentDto>> TableHeads { get; set; }
        public bool IsAdd { get; set; }
        public bool IsSelect { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
    }
}
