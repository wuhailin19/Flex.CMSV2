namespace Flex.Domain.Dtos.System.SiteManage
{
    public class SiteManageColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center, maxWidth = "80")]
        public int Id { set; get; }
        [ToolAttr(NameAttr = "站点名")]
        public string SiteName { set; get; }
        [ToolAttr(NameAttr = "站点描述")]
        public string? SiteDesc { set; get; }
        [ToolAttr(NameAttr = "数据表前缀")]
        public string TablePrefix { set; get; }
        [ToolAttr(NameAttr = "自定义路由")]
        public string RoutePrefix { set; get; }
        [ToolAttr(NameAttr = "目标路由")]
        public string TargetRoutePrefix { set; get; }
        [ToolAttr(HideFiled = true)]
        public string CopySiteId { set; get; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right)]
        public controlType Operation { get; set; }
    }
}
