using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminTokenInfoDto
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
