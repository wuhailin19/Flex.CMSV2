using Flex.Application.Aop;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices;
using Flex.Application.Contracts.IServices.System;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Enums.LogLevel;
using Flex.EFSql.UnitOfWork;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinExpressBusinessAccountGetAllResponse.Types;

namespace Flex.Application.Services
{
    public class AccountServices : BaseService, IAccountServices
    {
        private ICaching _caching;
        private IRoleServices _roleServices;
        private IMessageServices _msgServices;
        protected ISystemLogServices _logServices;
        protected IHubNotificationService _notificationService;
        private IAdminServices _adminServices;
        public AccountServices(IUnitOfWork unitOfWork
            , IMapper mapper, IdWorker idWorker, IClaimsAccessor claims
            , ICaching caching, ISystemLogServices logServices, IRoleServices roleServices, IMessageServices msgServices
            , IHubNotificationService notificationService, IAdminServices adminServices
            )
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _caching = caching;
            _logServices = logServices;
            _roleServices = roleServices;
            _msgServices = msgServices;
            _notificationService = notificationService;
            _adminServices = adminServices;
        }



        /// <summary>
        /// 判断验证码
        /// </summary>
        /// <returns></returns>
        public bool CheckAuthCode(AuthCodeInputDto authCodeInput)
        {
            if (authCodeInput.CodeId.IsNullOrEmpty())
                return false;
            if (_caching.Get(authCodeInput.CodeId) == null)
                return false;
            if (_caching.Get(authCodeInput.CodeId).ToString() != authCodeInput.Codenum)
            {
                _caching.Remove(authCodeInput.CodeId);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 通过微博ID进行登录操作
        /// </summary>
        /// <param name="weiboid"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<UserData>> GetAccountbyWeiboAsync(string weiboid)
        {
            var admin = await _adminServices.GetAdminByWeiboId(weiboid);
            var RolesName = string.Empty;

            if (admin == null)
            {
                var addadmin = new AdminAddDto();
                addadmin.Account = weiboid;
                addadmin.UserName = $"微博用户{weiboid}";
                addadmin.Password = weiboid;
                addadmin.UserAvatar = string.Empty;
                addadmin.RoleId = 1;
                addadmin.RoleName = "微博账号";
                addadmin.UserSign = $"{weiboid}的后台";
                addadmin.ErrorCount = 0;
                addadmin.WeiboId = weiboid;
                addadmin.MaxErrorCount = 5;
                addadmin.AllowMultiLogin = true;
                addadmin.Islock = false;
                var addresult = await _adminServices.InsertAdminReturnEntity(addadmin);
                if (!addresult.IsSuccess)
                {
                    return Problem<UserData>(HttpStatusCode.BadRequest, addresult.Detail);
                }
                admin = addresult.Content;
            }
            else
            {
                if (admin.RoleId == 0)
                    RolesName = "超级管理员";
                else
                {
                    var role = await _roleServices.GetRoleByIdAsync(admin.RoleId);
                    RolesName = role.RolesName;
                }
            }
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
            userdata.UserRoleName = RolesName;

            return Problem(HttpStatusCode.OK, userdata);
        }
        /// <summary>
        /// 根据ID获取UserData
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<UserData>> GetAccountValidateInfoAsync(long id)
        {
            var admin_unit = _unitOfWork.GetRepository<SysAdmin>();
            var admin = await admin_unit.GetFirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
                return new ProblemDetails<UserData>(HttpStatusCode.BadRequest, "认证失败");
            var userdata = _mapper.Map<UserData>(admin);
            var roleId = userdata.RoleId.ToInt();
            if (roleId != 0)
            {
                var role = await _roleServices.GetRoleByIdAsync(roleId);
                if (role == null)
                    return new ProblemDetails<UserData>(HttpStatusCode.BadRequest, "认证失败");
                userdata.UserRoleName = role.RolesName;
            }
            else
            {
                userdata.UserRoleName = "超级管理员";
            }
            return new ProblemDetails<UserData>(HttpStatusCode.OK, userdata);
        }
        private async Task<ProblemDetails<UserData>> DecryptStringObj(string StringObj)
        {
            try
            {
                StringObj = EncryptHelper.RsaDecrypt(StringObj, RSAHepler.RSAPrivateKey);
                return Problem<UserData>(HttpStatusCode.OK, StringObj);
            }
            catch (Exception ex)
            {
                await _logServices.AddLoginLog(new LoginSystemLogDto()
                {
                    systemLogLevel = SystemLogLevel.Error,
                    operationContent = $"密码解析失败，密文为{StringObj}，密钥为{RSAHepler.RSAPrivateKey}"
                });

                return Problem<UserData>(HttpStatusCode.InternalServerError, "解析失败，重试", ex);
            }
        }
        private async Task<ProblemDetails<UserData>> CheckPasswordAsync(SysAdmin admin, string Password)
        {
            if (admin is null)
            {
                return Problem<UserData>(HttpStatusCode.BadRequest, ErrorCodes.AccountOrPwdWrong.GetEnumDescription());
            }
            if (admin.Islock && !admin.IsSystem)
            {
                return Problem<UserData>(HttpStatusCode.Locked, ErrorCodes.AccountDisabled.GetEnumDescription() + "，请联系管理员解封");
            }
            if (admin.ExpiredTime <= Clock.Now)
            {
                return Problem<UserData>(HttpStatusCode.Locked, ErrorCodes.AccountExpried.GetEnumDescription() + "，请联系管理员解封");
            }

            if (admin.LockTime != null)
            {
                var locktime = admin.LockTime - Clock.Now;
                if (locktime > TimeSpan.Zero && !admin.IsSystem)
                {
                    var lockstr = $"{locktime?.Minutes}分{locktime?.Seconds}秒";
                    return Problem<UserData>(HttpStatusCode.Locked, ErrorCodes.AccountLocked.GetEnumDescription() + $"，请{lockstr}后再尝试");
                }
            }
            var result = await DecryptStringObj(Password);
            if (result.IsSuccess)
            {
                Password = result.Detail;
            }
            else
            {
                return result;
            }
            if (admin.Password != EncryptHelper.MD5Encoding(Password, admin.SaltValue))
            {
                if (!admin.IsSystem)
                {
                    admin.ErrorCount = admin.ErrorCount + 1;
                    var msg = "密码不正确，还有" + (admin.MaxErrorCount - admin.ErrorCount) + "次机会";
                    if (admin.MaxErrorCount - admin.ErrorCount == 0)
                    {
                        //admin.Islock = true;
                        admin.ErrorCount = 0;
                        admin.LockTime = Clock.Now.AddMinutes(30);
                        msg = "该账户已被锁定，请联系超级管理员解锁，或者等待半小时解锁";
                    }
                    _unitOfWork.SetTransaction();
                    _unitOfWork.GetRepository<SysAdmin>().Update(admin);
                    await _unitOfWork.SaveChangesTranAsync();

                    await _logServices.AddLoginLog(new LoginSystemLogDto()
                    {
                        systemLogLevel = SystemLogLevel.Warning,
                        operationContent = msg,
                        inoperator = $"{admin.UserName}({admin.Id})",
                        IsAuthenticated = true,
                        UserId = admin.Id,
                        UserName = admin.UserName
                    });
                    return Problem<UserData>(HttpStatusCode.BadRequest, msg);
                }
                else
                {
                    await _logServices.AddLoginLog(new LoginSystemLogDto() { systemLogLevel = SystemLogLevel.Warning, operationContent = ErrorCodes.AccountOrPwdWrong.GetEnumDescription(), inoperator = $"{admin.UserName}({admin.Id})" });
                    return Problem<UserData>(HttpStatusCode.BadRequest, ErrorCodes.AccountOrPwdWrong.GetEnumDescription());
                }
            }
            return Problem<UserData>(HttpStatusCode.OK, "");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="adminLoginDto"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<UserData>> LoginAuthorAsync(AdminLoginDto adminLoginDto)
        {
            if (adminLoginDto.CodeId.IsNotNullOrEmpty())
                _caching.Remove(adminLoginDto.CodeId);//删除验证码
            if (adminLoginDto.Account.IsNullOrEmpty() || adminLoginDto.Password.IsNullOrEmpty())
                return Problem<UserData>(HttpStatusCode.BadRequest, ErrorCodes.AccountOrPwdEmpty.GetEnumDescription());
            var admin_unit = _unitOfWork.GetRepository<SysAdmin>();
            var Account = string.Empty;
            var Password = string.Empty;
            ProblemDetails<UserData> result = await DecryptStringObj(adminLoginDto.Account);
            if (!result.IsSuccess)
            {
                return result;
            }
            Account = result.Detail;
            if (!admin_unit.Exists(m => m.Account == Account))
            {
                return Problem<UserData>(HttpStatusCode.BadRequest, ErrorCodes.AccountOrPwdWrong.GetEnumDescription());
            }
            var admin = await admin_unit.GetFirstOrDefaultAsync(m => m.Account == Account);

            result = await CheckPasswordAsync(admin, adminLoginDto.Password).ConfigureAwait(false);
            if (!result.IsSuccess)
            {
                return result;
            }
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
            AdminLoginLog loginLog = new AdminLoginLog();
            loginLog.CurrentLoginTime = DateTime.Now;
            loginLog.CurrentLoginIP = AcbHttpContext.ClientIp;
            //登录成功后的操作
            if (admin.LoginLogString.IsNullOrEmpty())
            {
                loginLog.LastLoginTime = admin.CurrentLoginTime;
                loginLog.LastLoginIP = admin.CurrentLoginIP;
            }
            else
            {
                var lastloginLog = JsonHelper.Json<AdminLoginLog>(admin.LoginLogString);
                loginLog.LastLoginTime = lastloginLog.CurrentLoginTime;
                loginLog.LastLoginIP = lastloginLog.CurrentLoginIP;
            }
            admin.LoginLogString = JsonHelper.ToJson(loginLog);
            admin.CurrentLoginIP = AcbHttpContext.ClientIp;
            admin.CurrentLoginTime = DateTime.Now;
            admin.LoginCount += 1;
            admin.LockTime = null;
            admin.ErrorCount = 0;
            if (admin.PwdExpiredTime.IsNotNullOrEmpty() && admin.PwdUpdateTime != null)
            {
                if (admin.PwdUpdateTime < Clock.Now)
                    return Problem<UserData>(HttpStatusCode.BadRequest, ErrorCodes.PwdExpried.GetEnumDescription());
                var time = admin.PwdUpdateTime - Clock.Now;
                var days = time?.Days;
                if (days <= 3)
                {
                    await _msgServices.SendNormalMsg("密码即将过期", $"密码将于{admin.PwdUpdateTime}过期，请及时修改，当前还剩{days}天{time?.Hours}时", admin.Id);
                }
            }
            _unitOfWork.SetTransaction();
            admin_unit.Update(admin);
            await _unitOfWork.SaveChangesTranAsync().ConfigureAwait(false);
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
            userdata.UserRoleName = RolesName;
            return Problem(HttpStatusCode.OK, userdata);
        }
    }
}
