using Flex.Core.JsonConvertExtension;
using Newtonsoft.Json;

namespace Flex.Domain.Dtos.Role
{
    public class RoleColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", maxWidth = "80")]
        [JsonConverter(typeof(IdToStringConverter))]
        public int Id { get; set; }
        [ToolAttr(NameAttr = "角色名", minWidth = "80")]
        public string RolesName { get; set; }
        [ToolAttr(NameAttr = "角色描述", maxWidth = "100")]
        public string? RolesDesc { get; set; }
        [ToolAttr(NameAttr = "创建者", maxWidth = "100")]
        public string? AddUserName { get; set; }
        [ToolAttr(NameAttr = "创建时间", maxWidth = "200")]
        public DateTime AddTime { get; set; }
        [ToolAttr(NameAttr = "修改人")]
        public string? LastEditUserName { get; set; }
        [ToolAttr(NameAttr = "修改时间", maxWidth = "200")]
        public DateTime? LastEditDate { get; set; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, minWidth = "500")]
        public controlType Operation { get; set; }
    }
}
