using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities.Normal
{
    public class norProductDetail : BaseIntEntity, EntityContext
    {
        public string Title { set; get; }
        public string Participants { set; get; }
        /// <summary>
        /// 修改内容
        /// </summary>
        public string Content { set; get; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplete { set; get; } = false;
        public int ProjectId { set; get; }
    }
}
