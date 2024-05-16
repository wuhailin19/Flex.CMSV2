using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminAddDto
    {
        [Required(ErrorMessage = "账号不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "账号的长度必须在3到50个字符之间")]
        public string Account { get; set; }
        [Required(ErrorMessage = "昵称不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "昵称的长度必须在3到50个字符之间")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码的长度必须在6到20个字符之间")]
        public string Password { get; set; }
        public string UserAvatar { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserSign { get; set; }
        public int ErrorCount { get; set; }
        public int MaxErrorCount { get; set; }
        public bool AllowMultiLogin { get; set; }
        public bool Islock { get; set; }
        public string? PwdExpiredTime { get; set; }
        public string FilterIp { set; get; }
        public DateTime? ExpiredTime { get; set; }
    }
}
