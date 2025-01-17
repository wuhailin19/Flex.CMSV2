﻿using Microsoft.Extensions.Options;
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

        public JwtService()
        {
            _jwtSetting = nameof(JwtSetting).Config<JwtSetting>();
            _tokenLifeTime = TimeSpan.FromMinutes(_jwtSetting.LifeTime);
            _refreshtokenLifeTime = TimeSpan.FromMinutes(_jwtSetting.RefreshTokenExpire);
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
        /// get UserId from refesh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public string GetUserIdFromRefeshToken(string refreshToken)
        {
            var token =  ReadToken(refreshToken);
            if (token is null)
            {
                return string.Empty;
            }
            var expirationtime = DateTime.Parse(token.Claims.First(m => m.Type == ClaimTypes.Expiration)?.Value ?? Clock.Now.ToString());
            if (expirationtime < DateTime.Now)
                return string.Empty;
            var UserId = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
            return UserId;
        }

		/// <summary>
		/// get UserData from  token
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public Claim GetUserDataFromToken(string Token)
		{
			var token = ReadToken(Token);
			if (token is null)
			{
				return null;
			}
			return token.Claims.FirstOrDefault();
		}

		/// <summary>
		/// 生成主Token
		/// </summary>
		/// <param name="userData">用户数据</param>
		/// <returns></returns>
		public string CreateAccessToken(UserData userData)
        {
            // 配置用户标识
            var userClaims = new Claim[]
            {
                new Claim(UserClaimType.Id,userData.Id.ToString()),//id
                new Claim(UserClaimType.Account,userData.Account),//account
                new Claim(UserClaimType.Name,userData.UserName),//name
                new Claim(UserClaimType.RoleId,userData.RoleId),//roleId
                new Claim(UserClaimType.RoleDisplayName,userData.UserRoleName),//rolename
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                //这个就是过期时间，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (ClaimTypes.Expiration,DateTime.Now.Add(_tokenLifeTime).ToString()),
            };

            return BuildToken(userClaims, Tokens.AccessToken);
        }
        /// <summary>
        /// 生成刷新Token
        /// </summary>
        /// <param name="userData">用户数据</param>
        /// <returns></returns>
        public string CreateRefreshToken(UserData userData)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userData.Id.ToString()),
                new Claim (ClaimTypes.Expiration,DateTime.Now.Add(_refreshtokenLifeTime).ToString()),
            };
            return BuildToken(claims, Tokens.RefreshToken);
        }
        /// <summary>
        /// 生成jwt令牌
        /// </summary>
        /// <param name="claims">自定义的claim</param>
        /// <param name="tokens"></param>
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
