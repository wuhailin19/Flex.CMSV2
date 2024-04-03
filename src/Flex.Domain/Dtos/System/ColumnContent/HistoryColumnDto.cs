using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class HistoryColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center, maxWidth = "80")]
        public int Id { set; get; }
        [ToolAttr(NameAttr = "标题", SortAttr = true, minWidth = "200")]
        public string Title { set; get; }
        [ToolAttr(NameAttr = "修改时间", SortAttr = true, minWidth = "200")]
        public string LastEditDate { set; get; }
        [ToolAttr(NameAttr = "修改人", minWidth = "100")]
        public string LastEditUserName { set; get; }
        [ToolAttr(NameAttr = "状态", Toolbar = "#statusPxy", SortAttr = true, AlignAttr = AlignEnum.Center, maxWidth = "100")]
        public string StatusCode { set; get; }
    }
}
