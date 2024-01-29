namespace Flex.Domain.Entities
{
    public class SysRoleUrl: BaseEntity, EntityContext
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ReturnContent { get; set; }
        public string? RequestType { get; set; }
        public int MaxErrorCount { get; set; }
        public int Category { get; set; }
        public bool NeedActionPermission { get; set; }
        public int OrderId { get; set; }
    }
}
