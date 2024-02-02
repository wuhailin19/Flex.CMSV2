using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.WorkFlow
{
    public class WorkFlowColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "编号")]
        public int Id { get; set; }
        [ToolAttr(NameAttr = "流程名")]
        public string Name { get; set; }
        [ToolAttr(NameAttr = "流程描述")]
        public string? Introduction { get; set; }
        [ToolAttr(NameAttr = "添加时间")]
        public DateTime AddTime { get; set; }
        [ToolAttr(NameAttr = "创建人")]
        public string? AddUserName { set; get; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right, minWidth = "200")]
        public controlType Operation { get; set; }
    }
}
