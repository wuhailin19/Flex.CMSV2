using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.FileManage
{
    public class ChangeFileContentQueryDto
    {
        public string path { set; get; }
        public string content { set; get; }
    }
}
