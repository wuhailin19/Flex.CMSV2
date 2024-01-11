using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class ColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center,Width ="80")]
        public int Id { set; get; }
        [ToolAttr(HideFiled = true)]
        public int SiteId { set; get; }
        [ToolAttr(HideFiled = true)]
        public int ModelId { set; get; }
        [ToolAttr(HideFiled = true)]
        public int ParentId { set; get; }
        [ToolAttr(NameAttr = "栏目名")]
        public string Name { set; get; }
        [ToolAttr(NameAttr = "链接")]
        public string ColumnUrl { set; get; }
        [ToolAttr(NameAttr = "显示状态", Toolbar = "#statusPxy", AlignAttr = AlignEnum.Center, Width = "100")]
        public string IsShow { set; get; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right)]
        public controlType Operation { get; set; }
    }
}
