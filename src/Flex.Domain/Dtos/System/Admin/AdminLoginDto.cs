using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminLoginDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码缓存key
        /// </summary>
        public string CodeId { get; set; }
        /// <summary>
        /// 验证码值
        /// </summary>
        public string Codenum { get; set; }
    }
}
