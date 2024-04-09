using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.ContentModel
{
    public class ContentModelColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center, maxWidth = "80")]
        public int Id { set; get; }
        [ToolAttr(NameAttr = "模型名")]
        public string Name { set; get; }
        [ToolAttr(NameAttr = "模型描述")]
        public string? Description { set; get; }
        [ToolAttr(NameAttr = "数据表名")]
        public string TableName { set; get; }
        [ToolAttr(NameAttr = "是否私有",Toolbar = "#SelfUse")]
        public bool SelfUse { set; get; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right)]
        public controlType Operation { get; set; }
    }
}
