using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Normal.ProductManage
{
    public class AddRecordDto
    {
        /// <summary>
        /// 修改标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 参与者
        /// </summary>
        public string Participants { set; get; }
        /// <summary>
        /// 修改信息
        /// </summary>
        public string? Content { set; get; }
        public int ProjectId { set; get; }
    }
}
