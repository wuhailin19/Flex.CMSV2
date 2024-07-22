using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.ColumnContent
{
    public class ContentImpoertExcelDto
    {
        public Dictionary<string, string> Fileds { get; set; } = new Dictionary<string, string>();
        public List<string> ImportColumns { get; set; }=new List<string>();
    }
}
