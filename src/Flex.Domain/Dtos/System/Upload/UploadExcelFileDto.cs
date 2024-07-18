using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.Upload
{
    public class UploadExcelFileDto
    {
        public IFormFileCollection file { set; get; }
        public int ParentId { set; get; }
        public int ModelId { set; get; }
        public long UserId { set; get; }
        public int PId { set; get; } = 0;
    }
}
