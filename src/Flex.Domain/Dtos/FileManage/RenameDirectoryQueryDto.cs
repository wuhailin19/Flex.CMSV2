using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.FileManage
{
    public class RenameDirectoryQueryDto
    {
        /// <summary>
        /// 当前路径
        /// </summary>
        public string path { set; get; }
        /// <summary>
        /// 当前文件夹
        /// </summary>
        public string folder { set; get; }
        /// <summary>
        /// 新名称
        /// </summary>
        public string newName { set; get; }
        public string type { set; get; }
    }
}
