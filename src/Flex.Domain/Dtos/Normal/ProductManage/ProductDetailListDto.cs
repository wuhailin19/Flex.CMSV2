using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Normal.ProductManage
{
    public class ProductDetailListDto
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Participants { set; get; }
        public DateTime AddTime{ set; get; }
        /// <summary>
        /// 修改内容
        /// </summary>
        public string Content { set; get; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplete { set; get; } = false;
    }
}
