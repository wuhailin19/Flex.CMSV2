using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities.System
{
    public class sysCmsConfig : BaseIntEntity, EntityContext
    {
        /// <summary>
        /// 后台名称
        /// </summary>
        public string CmsName { set; get; }
         
    }
}
