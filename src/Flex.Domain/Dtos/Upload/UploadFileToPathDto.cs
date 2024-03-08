using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Upload
{
    public class UploadFileToPathDto
    {
        public IFormFileCollection file { set; get; }
        public string path { set; get; }
    }
}
