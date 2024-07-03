using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class ColumnSortListDto
    {
        public int Id { set; get; }
        public int ParentId { set; get; }
        public string Name { set; get; }
        public int OrderId { set; get; }
    }
}
