using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Admin
{
    public class AccountAndPasswordDto
    {
        [Required(ErrorMessage = "账号不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "账号的长度必须在3到50个字符之间")]
        public string Account { get; set; }
        public string Password { get; set; }
        public int Version { get; set; }
    }
}
