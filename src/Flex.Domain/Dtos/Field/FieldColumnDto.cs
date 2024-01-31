using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.ContentModel
{
    public class FieldColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center, maxWidth = "80")]
        public string Id { set; get; }
        [ToolAttr(NameAttr = "字段名称")]
        public string FieldName { set; get; }
        [ToolAttr(NameAttr = "字段含义")]
        public string Name { set; get; }
        [ToolAttr(NameAttr = "字段描述")]
        public string? FieldDescription { set; get; }
        [ToolAttr(NameAttr = "字段类型")]
        public string FieldType { set; get; }
        [ToolAttr(NameAttr = "接口字段", Toolbar = "#apistatusPxy", AlignAttr = AlignEnum.Center, maxWidth = "100")]
        public bool IsApiField { set; get; }
        [ToolAttr(NameAttr = "搜索字段", Toolbar = "#searchstatusPxy", AlignAttr = AlignEnum.Center, maxWidth = "100")]
        public bool IsSearch { set; get; }
        [ToolAttr(NameAttr = "表格显示", Toolbar = "#tablethstatusPxy", AlignAttr = AlignEnum.Center, maxWidth = "100")]
        public bool ShowInTable { set; get; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right, HideFiled = true)]
        public controlType Operation { get; set; }
    }
}
