using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Role
{
    public class sitePermissionDto
    {
        public int siteId { set; get; }

        public DataPermissionDto columnPermission { set; get; }
    }
    public class DataPermissionDto
    {
        /// <summary>
        /// 修改
        /// </summary>
        public string sp { set; get; }
        /// <summary>
        /// 新增
        /// </summary>
        public string ad { set; get; }
        /// <summary>
        /// 编辑
        /// </summary>
        public string ed { set; get; }
        /// <summary>
        /// 删除
        /// </summary>
        public string dp { set; get; }
    }
}
