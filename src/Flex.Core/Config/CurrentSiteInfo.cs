using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Config
{
    public class CurrentSiteInfo
    {
        public static int SiteId { get; set; }
        /// <summary>
        /// 站点文件夹
        /// </summary>
        public static string? SiteUploadPath
        {
            get
            {
                if (SiteId != 0)
                {
                    return $"/site" + SiteId;
                }
                else { return string.Empty; }
            }
        }
    }
}
