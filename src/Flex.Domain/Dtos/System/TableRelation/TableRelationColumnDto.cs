using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.TableRelation
{
    public class TableRelationColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        [SugarColumn(IsIgnore = true)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center)]
        public int Id { get; set; }
        [ToolAttr(NameAttr = "父级模型")]
        public int ParentModelId { get; set; }
      
        [ToolAttr(NameAttr = "子级模型ID")]
        public int ChildModelId { get; set; }
        [ToolAttr(NameAttr = "链接名字")]
        public string LinkName { get; set; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right, minWidth = "200")]
        public controlType Operation { get; set; }
    }
}
