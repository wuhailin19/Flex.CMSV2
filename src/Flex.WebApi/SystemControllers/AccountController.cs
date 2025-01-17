﻿using Flex.Application.Jwt;
using Flex.Core.Admin.Application;
using Flex.Core.Attributes;
using Flex.Core.Helper;
using Flex.Core.Helper.MemoryCacheHelper;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.AuthCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShardingCore.Extensions;

namespace Flex.WebApi.SystemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "登录相关接口", IsFilter = true)]
    public class AccountController : ApiBaseController
    {
        private IAdminServices _services;
        private JwtService _jwtservice;
        private ICaching _caching;
        private IAccountServices _accountservices;
        public AccountController(IAdminServices services, IAccountServices accountservices, JwtService jwtservice, ICaching caching)
        {
            _services = services;
            _jwtservice = jwtservice;
            _accountservices = accountservices;
            _caching = caching;
        }
        /// <summary>
        /// 验证码判断
        /// </summary>
        /// <param name="authCodeInput"></param>
        /// <returns></returns>
        [HttpPost("CheckAuthCode")]
        [AllowAnonymous]
        public string CheckAuthCode(AuthCodeInputDto authCodeInput)
        {
            if (!_accountservices.CheckAuthCode(authCodeInput))
                return Fail("验证码错误，请重新输入！");
            return Success("");
        }
        /// <summary>
        /// 随机验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetAuthCode")]
        public string GetAuthCode()
        {
            var codeid = Guid.NewGuid().ToString();
            string codeimg = AuthCodeHelper.CreateCheckCodeImage(out string str);
            //存入缓存
            _caching.Set(codeid, str, 30);
            return Success(new AuthCodeOutputDto() { CodeId = codeid, ImageCode = codeimg, Publickey = RSAHepler.RSAPublicKey });
        }

        /// <summary>
        /// 测试获取分页Admin信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        [Descriper(IsFilter = true)]
        public async Task<string> GetPageListAsync(int pageSize = 10)
        {
            var list = await _services.GetPageListAsync(pageSize);
            return Success(list);
        }

        /// <summary>
        /// 使用刷新Token获取活动Token
        /// </summary>
        /// <param name="refreshTokenDto"></param>
        /// <returns></returns>
        [HttpPost("RefreshAccessTokenAsync")]
        [Descriper(IsFilter = true)]
        public async Task<string> RefreshAccessTokenAsync([FromBody] AdminRefreshTokenDto refreshTokenDto)
        {
            if (refreshTokenDto.RefreshToken.IsEmpty() || refreshTokenDto.AccessToken.IsEmpty())
                return Fail("请重新登录");
            var UserId = _jwtservice.GetUserIdFromRefeshToken(refreshTokenDto.RefreshToken);
            if (UserId.IsEmpty())
                return Fail("请重新登录");
            var result = await _accountservices.GetAccountValidateInfoAsync(UserId.ToLong());
            if (!result.IsSuccess)
                return Fail("请重新登录");
            var tokenmodel = new AdminTokenInfoDto
            {
                AccessToken = _jwtservice.CreateAccessToken(result.Content),
                RefreshToken = refreshTokenDto.RefreshToken
            };
            return NotFound();
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet("EcryptPwd")]
        [Descriper(IsFilter = true)]
        public string EcryptPwd(string pwd)
        => EncryptHelper.RsaEncrypt(pwd, RSAHepler.RSAPrivateKey);
        /// <summary>
        /// 测试登录
        /// </summary>
        /// <param name="adminLoginDto"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost("LoginAsync")]
        [Descriper(IsFilter = true)]
        public async Task<string> LoginAsync([FromBody] AdminLoginDto adminLoginDto)
        {
            if (!_accountservices.CheckAuthCode(new AuthCodeInputDto()
            {
                CodeId = adminLoginDto.CodeId,
                Codenum = adminLoginDto.Codenum
            }))
                return Fail("验证码错误，请重新输入！");
            var result = await _accountservices.LoginAuthorAsync(adminLoginDto);
            if (result is null)
                return Fail("登录错误");
            if (result.IsSuccess)
            {
                return Success(
                    new AdminTokenInfoDto
                    {
                        AccessToken = _jwtservice.CreateAccessToken(result.Content),
                        RefreshToken = _jwtservice.CreateRefreshToken(result.Content)
                    });
            }
            return Fail(result.Detail);
        }
    }
}
