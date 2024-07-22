using Flex.Core.Helper;
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
        public List<FieldDataCIoumnRelate> FieldDict
        {
            get
            {

                return FieldDictStr.IsNullOrEmpty() ? new List<FieldDataCIoumnRelate>() : JsonHelper.Json<List<FieldDataCIoumnRelate>>(FieldDictStr);
            }
        }
        public string? FieldDictStr { set; get; }
        public int ParentId { set; get; }
        public int ModelId { set; get; }
        public long UserId { set; get; }
        public int PId { set; get; } = 0;

        public byte[]? FileContent { get; set; }
    }
    public class FieldDataCIoumnRelate
    {
        public string field { set; get; }
        public string value { set; get; }
    }
}
