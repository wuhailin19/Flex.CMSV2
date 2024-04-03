using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.Menu
{
    public class MenuColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox, Width = "80")]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center, Width = "80")]
        public int ID { get; set; }
        [ToolAttr(NameAttr = "父级", HideFiled = true)]
        public int ParentID { get; set; }
        [ToolAttr(NameAttr = "菜单", AlignAttr = AlignEnum.Left)]
        public string Name { get; set; }
        [ToolAttr(NameAttr = "图标", Toolbar = "#iconPxy", AlignAttr = AlignEnum.Center, Width = "100")]
        public string Icode { get; set; }
        [ToolAttr(NameAttr = "显示状态", Toolbar = "#statusPxy", AlignAttr = AlignEnum.Center, Width = "100")]
        public bool Status { get; set; }
        [ToolAttr(NameAttr = "目录状态", Toolbar = "#menuPxy", AlignAttr = AlignEnum.Center, Width = "100")]
        public bool isMenu { get; set; }
        [ToolAttr(NameAttr = "链接类型", Toolbar = "#controllerPxy", AlignAttr = AlignEnum.Center, Width = "100", HideFiled = true)]
        public bool IsControllerUrl { get; set; }
        [ToolAttr(NameAttr = "链接地址")]
        public string LinkUrl { get; set; }
        [ToolAttr(NameAttr = "图标种类", HideFiled = true)]
        public string FontSort { get; set; }
        [ToolAttr(NameAttr = "等级", HideFiled = true)]
        public int Level { get; set; }
        [ToolAttr(NameAttr = "排序号", Width = "100", AlignAttr = AlignEnum.Center)]
        public int OrderId { get; set; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, SortAttr = Enable.True, Width = "200", Fixed = AlignEnum.Right)]
        public controlType Operation { get; set; }
    }
}
