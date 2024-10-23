using AutoMapper;
using Flex.Application.Contracts.IServices.System;
using Flex.Application.Jwt;
using Flex.Application.SignalRBus.Hubs;
using Flex.Core.Attributes;
using Flex.Core.Helper;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Entities.System;
using Flex.Domain.Enums.LogLevel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBDescribeCloudBaseRunEnvironmentsResponse.Types.Environment.Types;

namespace Flex.SingleWeb.Areas.System.ApiController
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "第三方登录相关接口", IsFilter = true)]
    public class OauthController : ApiBaseController
    {
        private IAdminServices _adminServices;
        private JwtService _jwtservice;
        private ISystemLogServices _logServices;
        private IMapper _mapper;
        private readonly ConnectionStatus _connectionStatus;
        private IRoleServices _roleServices;

        public OauthController(IAdminServices adminServices, ISystemLogServices logServices, JwtService jwtservice, IMapper mapper, ConnectionStatus connectionStatus, IRoleServices roleServices)
        {
            _adminServices = adminServices;
            _logServices = logServices;
            _jwtservice = jwtservice;
            _mapper = mapper;
            _connectionStatus = connectionStatus;
            _roleServices = roleServices;
        }
        [HttpGet("GetLoginPath")]
        [AllowAnonymous]
        public string GetLoginPath() {
            string url = "OAuthLogin:AuthUrl".Config(string.Empty) + "&redirect_uri="+ "OAuthLogin:Redirect_Uri".Config(string.Empty)+"###";
            return Success(url);
        }


        [HttpPost("WeiboLogin")]
        [AllowAnonymous]
        public async Task<string> WeiboLogin([FromForm] string code)
        {
            var url = "https://api.weibo.com/oauth2/access_token";
            var model = new AuthModel();
            model.client_id = "OAuthLogin:Weibo:AppKey".Config(string.Empty);
            model.client_secret = "OAuthLogin:Weibo:AppSecret".Config(string.Empty);
            model.grant_type = "authorization_code";
            model.code = code;
            model.redirect_uri = "OAuthLogin:Redirect_Uri".Config(string.Empty);


            url += $"?client_id={model.client_id}";
            url += $"&client_secret={model.client_secret}";
            url += $"&grant_type={model.grant_type}";
            url += $"&code={model.code}";
            url += $"&redirect_uri={model.redirect_uri}";
            var result = HttpHelper.Post(url, string.Empty);


            var result_model = JsonHelper.Json<AccessTokenModel>(result);

            var admin = await _adminServices.GetAdminByWeiboId(result_model.uid);
            if (admin == null)
            {
                var addadmin = new AdminAddDto();
                addadmin.Account = result_model.uid;
                addadmin.UserName = $"微博用户{result_model.uid}";
                addadmin.Password = result_model.uid;
                addadmin.UserAvatar = string.Empty;
                addadmin.RoleId = 1;
                addadmin.RoleName = "微博账号";
                addadmin.UserSign = $"{result_model.uid}的后台";
                addadmin.ErrorCount = 0;
                addadmin.WeiboId = result_model.uid;
                addadmin.MaxErrorCount = 5;
                addadmin.AllowMultiLogin = true;
                addadmin.Islock = false;
                var addresult = await _adminServices.InsertAdminReturnEntity(addadmin);
                if (!addresult.IsSuccess)
                {
                    return Fail(addresult.Detail);
                }
                admin = addresult.Content;
                await _logServices.AddLoginLog(new LoginSystemLogDto()
                {
                    systemLogLevel = SystemLogLevel.Normal,
                    operationContent = "登录成功",
                    inoperator = $"{admin.UserName}({admin.Id})",
                    IsAuthenticated = true,
                    UserId = admin.Id,
                    UserName = admin.UserName
                });
                var userdata = _mapper.Map<UserData>(admin);
                userdata.UserRoleName = addadmin.RoleName;

                var tokenmodel = new AdminTokenInfoDto
                {
                    AccessToken = _jwtservice.CreateAccessToken(userdata),
                    RefreshToken = _jwtservice.CreateRefreshToken(userdata)
                };

                _connectionStatus.AddOrUpdateConnection(JwtBearerDefaults.AuthenticationScheme + " " + tokenmodel.AccessToken, string.Empty);

                return Success(tokenmodel);
            }
            else
            {
                //Console.WriteLine(JsonHelper.ToJson(admin));
                await _logServices.AddLoginLog(new LoginSystemLogDto()
                {
                    systemLogLevel = SystemLogLevel.Normal,
                    operationContent = "登录成功",
                    inoperator = $"{admin.UserName}({admin.Id})",
                    IsAuthenticated = true,
                    UserId = admin.Id,
                    UserName = admin.UserName
                });
                var userdata = _mapper.Map<UserData>(admin);
                var RolesName = string.Empty;
                if (admin.RoleId == 0)
                {
                    RolesName = "超级管理员";
                }
                else
                {
                    var role = await _roleServices.GetRoleByIdAsync(admin.RoleId);
                    RolesName = role.RolesName;
                }
                userdata.UserRoleName = RolesName;
                var tokenmodel = new AdminTokenInfoDto
                {
                    AccessToken = _jwtservice.CreateAccessToken(userdata),
                    RefreshToken = _jwtservice.CreateRefreshToken(userdata)
                };
                _connectionStatus.AddOrUpdateConnection(JwtBearerDefaults.AuthenticationScheme + " " + tokenmodel.AccessToken, string.Empty);
                return Success(tokenmodel);
            }

        }
        private class AccessTokenModel
        {
            public string access_token { set; get; }
            public string expires_in { set; get; }
            public string remind_in { set; get; }
            public string uid { set; get; }
        }
        private class AuthModel
        {
            public string client_id { get; set; }
            public string client_secret { get; set; }
            public string grant_type { get; set; }
            public string code { get; set; }
            public string redirect_uri { get; set; }
        }
    }
}
