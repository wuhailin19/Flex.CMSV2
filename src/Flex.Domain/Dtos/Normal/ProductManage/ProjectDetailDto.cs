using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Normal.ProductManage
{
    public class ProjectDetailDto
    {
        public int Id { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProductName { set; get; }
        /// <summary>
        /// 参与者
        /// </summary>
        public string Participants { set; get; }
        /// <summary>
        /// 服务器信息
        /// </summary>
        public string ServerInfo { set; get; }
        public DateTime AddTime { set; get; }
    }
}
