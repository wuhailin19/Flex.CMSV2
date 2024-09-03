using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminRefreshTokenDto
    {
        /// <summary>
        /// RefreshToken
        /// </summary>
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
    }
}
