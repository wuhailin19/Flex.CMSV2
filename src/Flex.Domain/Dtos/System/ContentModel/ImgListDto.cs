using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.ContentModel
{
    public class ImgListDto
    {
        public string content { set; get; }
        public string title { set; get; }
        public string imgsrc { set; get; }
    }
    public class JsonDocxImages {
        public string img_content { set; get; }
        public string img_title { set; get; }
        public string img_src { set; get; }
    }
}
