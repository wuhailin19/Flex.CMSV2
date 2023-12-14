namespace Flex.Domain.Entities.System
{
    public class SysRole : BaseIntEntity, EntityContext
    {
        public string  RolesName { get; set; }
        public string? WebsitePermissions { get; set; }
        public string? MenuPermissions { get; set; }
        public string? DataPermission { get; set; }
        public string? UrlPermission { get; set; }
        public string? RolesDesc { get; set; }
        public long? GroupId { get; set; }
    }
}
