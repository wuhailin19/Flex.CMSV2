namespace Flex.Domain.Entities
{
    /// <summary>
    /// 部门/机构
    /// </summary>
    public class SysGroup: BaseLongEntity, EntityContext
    {
        public string GroupName { get; set; }
        public string WebsitePermissions { get; set; }
        public string MenuPermissions { get; set; }
        public string DataPermission { get; set; }
        public string UrlPermission { get; set; }
        public string GroupDesc { get; set; }
    }
}
