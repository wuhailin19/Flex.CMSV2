using AutoMapper;
using Flex.Application.Contracts.IServices.System;
using Flex.Application.Contracts.Oauth;
using Flex.Application.Jwt;
using Flex.Application.Services;
using Flex.Application.SignalRBus.Hubs;
using Flex.Core.Attributes;
using Flex.Core.Config;
using Flex.Core.Helper;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Enums.LogLevel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ApiController
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "第三方登录相关接口", IsFilter = true)]
    public class OauthController : ApiBaseController
    {
        private IAccountServices _accountServices;
        private JwtService _jwtservice;
        private readonly ConnectionStatus _connectionStatus;

        public OauthController(IAccountServices accountServices, JwtService jwtservice, ConnectionStatus connectionStatus)
        {
            _accountServices = accountServices;
            _jwtservice = jwtservice;
            _connectionStatus = connectionStatus;
        }

        /// <summary>
        /// 获取微博登录链接
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLoginPath")]
        [AllowAnonymous]
        public string GetLoginPath()
        {
            string url = OAuthLoginConfig.AuthUrl + "&redirect_uri=" + OAuthLoginConfig.Redirect_Uri + "###";
            return Success(url);
        }

        /// <summary>
        /// 微博进行授权
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("WeiboLogin")]
        [AllowAnonymous]
        public async Task<string> WeiboLogin([FromForm] string code)
        {
            var url = "https://api.weibo.com/oauth2/access_token";
            var model = new AuthModel();
            model.client_id = OAuthLoginConfig.AppKey;
            model.client_secret = OAuthLoginConfig.AppSecret;
            model.grant_type = "authorization_code";
            model.code = code;
            model.redirect_uri = OAuthLoginConfig.Redirect_Uri;

            url += $"?client_id={model.client_id}";
            url += $"&client_secret={model.client_secret}";
            url += $"&grant_type={model.grant_type}";
            url += $"&code={model.code}";
            url += $"&redirect_uri={model.redirect_uri}";
            var result = HttpHelper.Post(url, string.Empty);
            var result_model = JsonHelper.Json<AccessTokenModel>(result);

            if (result_model == null || result_model.uid.IsNullOrEmpty())
            {
                return Fail("认证失败");
            }
            var vaildate_result = await _accountServices.GetAccountbyWeiboAsync(result_model.uid);
            if (!vaildate_result.IsSuccess)
            {
                return Fail(vaildate_result.Detail);
            }
            var tokenmodel = new AdminTokenInfoDto
            {
                AccessToken = _jwtservice.CreateAccessToken(vaildate_result.Content),
                RefreshToken = _jwtservice.CreateRefreshToken(vaildate_result.Content)
            };
            _connectionStatus.AddOrUpdateConnection(JwtBearerDefaults.AuthenticationScheme + " " + tokenmodel.AccessToken, string.Empty);
            return Success(tokenmodel);
        }
    }
}
