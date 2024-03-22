using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.ColumnContent
{
    public class ContentPageListParamDto
    {
        public int page { set; get; }
        public int limit { set; get; }
        public int ParentId { set; get; }
        public string? ContentGroupId { set; get; }
        public string? k { set; get; } = null;
        public DateTime? timefrom { set; get; }
        public DateTime? timeto { set; get; }
    }
}
