using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.SiteManage
{
    public class AddSiteManageDto
    {
        public string SiteName { set; get; }
        public string? SiteDesc { set; get; }
        public string? TablePrefix { set; get; }
        public string? RoutePrefix { set; get; }
        public string? TargetRoutePrefix { set; get; }
        public int CopySiteId { set; get; } = 0;
    }
}
