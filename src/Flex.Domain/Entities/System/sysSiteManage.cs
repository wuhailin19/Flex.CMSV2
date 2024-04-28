using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities.System
{
    public class sysSiteManage : BaseIntEntity, EntityContext
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName { set; get; }
        /// <summary>
        /// 站点描述
        /// </summary>
        public string SiteDesc { set; get; }
        /// <summary>
        /// 数据表前缀，用于生成数据表时做区分
        /// </summary>
        public string TablePrefix { set; get; }
        /// <summary>
        /// 路由前缀
        /// </summary>
        public string RoutePrefix { set; get; }
        /// <summary>
        /// 目标路由
        /// </summary>
        public string TargetRoutePrefix { set; get; }
        /// <summary>
        /// 选择复制站点，为0则不复制
        /// </summary>
        public int CopySiteId { set; get; } = 0;
    }
}
