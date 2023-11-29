using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminRefreshTokenDto
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
