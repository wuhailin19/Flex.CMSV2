using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.FileManage
{
    public class ChangeDirectoryQueryDto
    {
        public string oldpath { set; get; }
        public string newpath { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        /// <summary>
        /// 默认覆盖为false
        /// </summary>
        public bool Isoverride { set; get; } = false;
    }
}
