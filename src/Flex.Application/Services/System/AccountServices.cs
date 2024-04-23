using Flex.Application.Aop;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices.System;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Enums.LogLevel;
using Flex.EFSql.UnitOfWork;

namespace Flex.Application.Services
{
    public class AccountServices : BaseService, IAccountServices
    {
        private ICaching _caching;
        private IRoleServices _roleServices;
        protected ISystemLogServices _logServices;
        public AccountServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims
            , ICaching caching, ISystemLogServices logServices, IRoleServices roleServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _caching = caching;
            _logServices = logServices;
            _roleServices = roleServices;
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
        private async Task<ProblemDetails<UserData>> DecryptStringObj(string StringObj)
        {
            try
            {
                StringObj = EncryptHelper.RsaDecrypt(StringObj, RSAHepler.RSAPrivateKey);
                return Problem<UserData>(HttpStatusCode.OK, StringObj);
            }
            catch (Exception ex)
            {
                await _logServices.AddLoginLog(new LoginSystemLogDto() { systemLogLevel = SystemLogLevel.Error, operationContent = $"密码解析失败，密文为{StringObj}，密钥为{RSAHepler.RSAPrivateKey}" });

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
                return Problem<UserData>(HttpStatusCode.Locked, ErrorCodes.AccountLocked.GetEnumDescription() + "，请联系管理员解锁");
            }
            if (admin.LockTime != null)
            {
                var locktime = admin.LockTime - Clock.Now;
                if (locktime > TimeSpan.Zero && !admin.IsSystem)
                {
                    var lockstr = $"{locktime?.Milliseconds}分{locktime?.Seconds}";
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

                    await _logServices.AddLoginLog(new LoginSystemLogDto() { systemLogLevel = SystemLogLevel.Warning, operationContent = msg, inoperator = admin.UserName });
                    return Problem<UserData>(HttpStatusCode.BadRequest, msg);
                }
                else
                {
                    await _logServices.AddLoginLog(new LoginSystemLogDto() { systemLogLevel = SystemLogLevel.Warning, operationContent = ErrorCodes.AccountOrPwdWrong.GetEnumDescription(), inoperator = admin.UserName });
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
            var admin = await admin_unit.GetFirstOrDefaultAsync(m => m.Account == Account, null, null, true, false);

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
            admin.ErrorCount = 0;
            _unitOfWork.SetTransaction();
            admin_unit.Update(admin);
            await _unitOfWork.SaveChangesTranAsync().ConfigureAwait(false);
            await _logServices.AddLoginLog(new LoginSystemLogDto() { systemLogLevel = SystemLogLevel.Normal, operationContent = "登录成功", inoperator = admin.UserName });
            var userdata = _mapper.Map<UserData>(admin);
            userdata.UserRoleName = RolesName;
            return Problem(HttpStatusCode.OK, userdata);
        }
    }
}
