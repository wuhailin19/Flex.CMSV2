using Flex.Core.JsonConvertExtension;
using Newtonsoft.Json;

namespace Flex.Domain.Dtos.Role
{
    public class RoleColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "ID", Width = "80")]
        [JsonConverter(typeof(IdToStringConverter))]
        public long Id { get; set; }
        [ToolAttr(NameAttr = "角色名", Width = "200")]
        public string RolesName { get; set; }
        [ToolAttr(NameAttr = "角色描述", Width = "200")]
        public string? RolesDesc { get; set; }
        [ToolAttr(NameAttr = "创建者", Width = "200")]
        public string? AddUserName { get; set; }
        [ToolAttr(NameAttr = "创建时间", Width = "200")]
        public DateTime AddTime { get; set; } 
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Width = "400")]
        public controlType Operation { get; set; }
    }
}
