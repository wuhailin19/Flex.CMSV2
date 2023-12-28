using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Application.Authorize
{
    public class JwtSetting
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 安全密钥
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public double LifeTime { get; set; }
        /// <summary>
        /// RefreshToken过期时间，单位分钟
        /// </summary>
        public double RefreshTokenExpire { get; set; }
    }
}
