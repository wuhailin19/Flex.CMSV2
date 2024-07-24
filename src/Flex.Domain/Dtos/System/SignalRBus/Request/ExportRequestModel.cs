namespace Flex.Domain.Dtos.SignalRBus.Model.Request
{
    public class ExportRequestModel : RequestModel
    {
        public int page { set; get; }
        public int limit { set; get; }
        public int ParentId { set; get; }
        public int ModelId { set; get; }
        public int PId { set; get; } = 0;
        public string? ContentGroupId { set; get; }
        public string? k { set; get; } = null;
        public DateTime? timefrom { set; get; }
        public DateTime? timeto { set; get; }
    }
}
