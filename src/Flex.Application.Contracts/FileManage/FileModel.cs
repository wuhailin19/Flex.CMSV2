using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.FileManage
{
    public class FileModel
    {
        public string name { set; get; }
        public string path { set; get; }
        public string filepath { set; get; }
        public string type { set; get; }
        public string thumb { set; get; }
        public int isdir { set; get; }
        public string length { set; get; }
    }
}
