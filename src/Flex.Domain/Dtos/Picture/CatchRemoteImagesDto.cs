using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Picture
{
    public class CatchRemoteImagesDto
    {
        /// <summary>
        /// 原图路径
        /// </summary>
        public string source { set; get; }
        /// <summary>
        /// 上传后新路径
        /// </summary>
        public string url { set; get; }
        /// <summary>
        /// 下载状态
        /// </summary>
        public string state { set; get; }
    }
}
