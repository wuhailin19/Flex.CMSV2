using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class ColumnDataPermissionDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "ID", AlignAttr = AlignEnum.Center, Width = "80", HideFiled = true)]
        public int Id { set; get; }
        [ToolAttr(NameAttr = "栏目名", Width = "300")]
        public string Name { set; get; }
        [ToolAttr(NameAttr = "权限分配", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right)]
        public controlType Operation { get; set; }
    }
}
