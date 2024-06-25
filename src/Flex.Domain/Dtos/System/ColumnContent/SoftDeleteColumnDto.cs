using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class SoftDeleteColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center, maxWidth = "80")]
        public int Id { set; get; }
        [ToolAttr(NameAttr = "标题", SortAttr = true, minWidth = "200")]
        public string Title { set; get; }
        [ToolAttr(NameAttr = "删除时间", SortAttr = true, minWidth = "200")]
        public string LastEditDate { set; get; }
        [ToolAttr(NameAttr = "删除人", minWidth = "100")]
        public string LastEditUserName { set; get; }
    }
}
