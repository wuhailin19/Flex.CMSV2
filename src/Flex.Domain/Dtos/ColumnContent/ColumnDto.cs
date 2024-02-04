using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class ColumnContentDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center, maxWidth = "80")]
        public int Id { set; get; }
        [ToolAttr(NameAttr = "标题", SortAttr = true, minWidth = "200")]
        public string Title { set; get; }
        [ToolAttr(NameAttr = "发布时间", SortAttr = true)]
        public string AddTime { set; get; }
        [ToolAttr(NameAttr = "排序号", SortAttr = true, AlignAttr = "center", maxWidth = "100")]
        public string OrderId { set; get; }
        [ToolAttr(NameAttr = "添加人")]
        public string AddUserName { set; get; }
        [ToolAttr(NameAttr = "最后编辑人")]
        public string LastEditUserName { set; get; }
        [ToolAttr(NameAttr = "状态", Toolbar = "#statusPxy", SortAttr = true, AlignAttr = AlignEnum.Center, maxWidth = "100")]
        public string StatusCode { set; get; }
    }
}
