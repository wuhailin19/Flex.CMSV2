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
        [ToolAttr(NameAttr = "编号", AlignAttr = AlignEnum.Center,maxWidth ="80")]
        public int Id { set; get; }
        [ToolAttr(NameAttr = "标题")]
        public string Title { set; get; }
        [ToolAttr(NameAttr = "发布时间")]
        public string AddTime { set; get; }
        [ToolAttr(NameAttr = "添加人")]
        public string AddUserName { set; get; }
        [ToolAttr(NameAttr = "最后编辑人")]
        public string LastEditUserName { set; get; }
        [ToolAttr(NameAttr = "状态", Toolbar = "#statusPxy", AlignAttr = AlignEnum.Center, maxWidth = "100")]
        public string StatusCode { set; get; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right)]
        public controlType Operation { get; set; }
    }
}
