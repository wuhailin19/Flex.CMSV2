using Flex.Core.Helper;
using Microsoft.AspNetCore.Http;

namespace Flex.Domain.Dtos.SignalRBus.Model.Request
{
    public class ImportRequestModel: RequestModel
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
        public int PId { set; get; } = 0;

        public byte[]? FileContent { get; set; }
    }
    public class FieldDataCIoumnRelate
    {
        public string field { set; get; }
        public string value { set; get; }
    }
}
