using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminUserViewModelDto
    {
        [Display(Name ="编号")]
        public long AdminId { get; set; }
        [Display(Name = "账号")]
        public string Account { get; set; }
        [Display(Name = "别名")]
        public string UserName { get; set; }
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Display(Name = "确认密码")]
        public string CheckPwd { get; set; }
    }
}
