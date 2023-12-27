using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class TreeColumnListDto
    {
        public int Id { set; get; }
        public int ParentId { set; get; }
        public string Name { set; get; }
        public bool IsShow { set; get; }
        public int OrderId { set; get; }
        public List<TreeColumnListDto> children { set; get; }
    }
}
