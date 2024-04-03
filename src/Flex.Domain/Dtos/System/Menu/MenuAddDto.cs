using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Menu
{
    public class MenuAddDto
    {
        public string FontSort { set; get; }
        public string Icode { set; get; }
        public bool IsControllerUrl { set; get; }
        public string LinkUrl { set; get; }
        public string Name { set; get; }
        public int OrderId { set; get; }
        public int ParentID { set; get; }
        public bool Status { set; get; }
        public bool isMenu { set; get; }
    }
}
