using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Flex.Application.Jwt
{
    public class JwtService
    {
        private readonly JwtSetting _jwtSetting;
        private readonly TimeSpan _tokenLifeTime;
        private readonly TimeSpan _refreshtokenLifeTime;

        public JwtService(IOptions<JwtSetting> options)
        {
            _jwtSetting = options.Value;
            _tokenLifeTime = TimeSpan.FromMinutes(options.Value.LifeTime);
            _refreshtokenLifeTime = TimeSpan.FromMinutes(options.Value.RefreshTokenExpire);
        }
        /*
             iss (issuer)：签发人
             exp (expiration time)：过期时间
             sub (subject)：主题
             aud (audience)：受众
             nbf (Not Before)：生效时间
             iat (Issued At)：签发时间
             jti (JWT ID)：编号
             */
        /// <summary>
        /// get account from refesh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public string GetAccountFromRefeshToken(string refreshToken)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
            if (token is null)
            {
                return string.Empty;
            }
            var claimAccount = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
            return claimAccount;
        }
        /// <summary>
        /// 生成主Token
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="roleName">登录时的角色</param>
        /// <returns></returns>
        public string CreateAccessToken(UserData userData)
        {
            // 配置用户标识
            var userClaims = new Claim[]
            {
                new Claim(UserClaimType.Id,userData.Id.ToString()),//id
                new Claim(UserClaimType.Account,userData.Account),//account
                new Claim(UserClaimType.Name,userData.UserName),//name
                new Claim(UserClaimType.RoleId,userData.RoleId),//rolename
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                //这个就是过期时间，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (ClaimTypes.Expiration,DateTime.Now.Add(_tokenLifeTime).ToString()),
            };

            return BuildToken(userClaims, Tokens.AccessToken);
        }
        /// <summary>
        /// 生成刷新Token
        /// </summary>
        /// <param name="jwtConfig"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateRefreshToken(UserData userData)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userData.Account),
                new Claim (ClaimTypes.Expiration,DateTime.Now.Add(_refreshtokenLifeTime).ToString()),
            };
            return BuildToken(claims, Tokens.RefreshToken);
        }
        /// <summary>
        /// 生成jwt令牌
        /// </summary>
        /// <param name="claims">自定义的claim</param>
        /// <returns></returns>
        public string BuildToken(Claim[] claims, Tokens tokens)
        {
            //var claims = BuildClaims(userData);
            var nowTime = DateTime.Now;
            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecurityKey)), SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokenkey = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                notBefore: nowTime,
                expires: nowTime.Add(_tokenLifeTime),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(tokenkey);
        }
        /// <summary>
        /// 读取Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public JwtSecurityToken ReadToken(string token) {
          return  new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
    }
}
