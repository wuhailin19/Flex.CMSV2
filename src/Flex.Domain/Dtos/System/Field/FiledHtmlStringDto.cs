using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Field
{
    public class FiledHtmlStringDto
    {
        public string id { set; get; }
        public int index { set; get; }
        public string uuid { set; get; }
        public string label { set; get; }
        public string tag { set; get; }
        public bool IsEdit { set; get; }
    }
}
