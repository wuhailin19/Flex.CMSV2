using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Normal.ProductManage
{
    public class ProjectListDto
    {
        public int Id { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProductName { set; get; }
    }
}
