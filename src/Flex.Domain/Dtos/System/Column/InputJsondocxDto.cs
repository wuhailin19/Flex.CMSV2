using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.Column
{
    public class InputJsondocxDto
    {
       public int modelId { set; get; }
        public int page { set; get; }
        public int pagesize { set; get; }
        public string columnId { set; get; }=string.Empty;
        public int PId { set; get; }
        public string k { set; get; } = string.Empty;
    }
}
