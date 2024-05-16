using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminEditDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "账号不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "账号的长度必须在3到50个字符之间")]
        public string Account { get; set; }
        [Required(ErrorMessage = "昵称不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "昵称的长度必须在3到50个字符之间")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserAvatar { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserSign { get; set; }
        public int ErrorCount { get; set; }
        public int MaxErrorCount { get; set; }
        public bool AllowMultiLogin { get; set; }
        public bool Islock { get; set; }
        public string FilterIp { set; get; }
        public DateTime? LockTime { get; set; }
        public DateTime? PwdUpdateTime { get; set; }
        public DateTime? ExpiredTime { get; set; }
        public int PwdExpiredTime { get; set; } = 0;
        public int Version { set; get; }
    }
}
