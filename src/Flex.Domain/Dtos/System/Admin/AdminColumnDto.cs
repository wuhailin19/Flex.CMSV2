using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminColumnDto
    {
        [ToolAttr(Fixed = AlignEnum.Left, Types = controlType.checkBox)]
        public controlType checkbox { get; set; }
        [ToolAttr(NameAttr = "ID", AlignAttr = AlignEnum.Center, HideFiled = true, maxWidth = "80")]
        public long Id
        {
            get; set;
        }
        [ToolAttr(NameAttr = "账号")]
        public string Account
        {
            get; set;
        }
        [ToolAttr(NameAttr = "昵称")]
        public string? UserName
        {
            get; set;
        }
        [ToolAttr(NameAttr = "上次登录IP")]
        public string? CurrentLoginIP
        {
            get; set;
        }
        [ToolAttr(NameAttr = "多用户登录", Toolbar = "#loginstatusPxy", AlignAttr = AlignEnum.Center)]
        public bool AllowMultiLogin
        {
            get; set;
        }
        [ToolAttr(NameAttr = "启用状态", Toolbar = "#lockstatusPxy", AlignAttr = AlignEnum.Center)]
        public bool Islock
        {
            get; set;
        }
        [ToolAttr(NameAttr = "解锁时间", AlignAttr = AlignEnum.Center)]
        public DateTime? LockTime
        {
            get; set;
        }
        [ToolAttr(NameAttr = "角色名")]
        public string? RoleName
        {
            get; set;
        }
        [ToolAttr(NameAttr = "登录次数")]
        public int LoginCount
        {
            get; set;
        }
        [ToolAttr(NameAttr = "创建时间（数据库）", HideFiled = true)]
        public DateTime? AddTime
        {
            get; set;
        }
        [ToolAttr(NameAttr = "创建人", HideFiled = true)]
        public string? AddUserName
        {
            get; set;
        }
        [ToolAttr(NameAttr = "用户头像", HideFiled = true)]
        public string? UserAvatar
        {
            get; set;
        }
        [ToolAttr(NameAttr = "登录错误次数")]
        public int ErrorCount { set; get; }
        [ToolAttr(NameAttr = "最大登录错误次数", HideFiled = true)]
        public int MaxErrorCount { set; get; }
        [ToolAttr(NameAttr = "操作", Toolbar = "#barDemo", AlignAttr = AlignEnum.Center, Fixed = AlignEnum.Right)]
        public controlType Operation { get; set; }
    }
}
