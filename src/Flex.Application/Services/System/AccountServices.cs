using Flex.EFSql.UnitOfWork;

namespace Flex.Application.Services
{
    public class AccountServices : BaseService, IAccountServices
    {
        private ICaching _caching;
        public AccountServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, ICaching caching)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _caching = caching;
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
        private ProblemDetails<UserData> DecryptStringObj(string StringObj)
        {
            try
            {
                StringObj = EncryptHelper.RsaDecrypt(StringObj, RSAHepler.RSAPrivateKey);
                return Problem<UserData>(HttpStatusCode.OK, StringObj);
            }
            catch
            {
                return Problem<UserData>(HttpStatusCode.RedirectKeepVerb, "解析失败，重试");
            }
        }
        private async Task<ProblemDetails<UserData>> CheckPasswordAsync(SysAdmin admin, string Password)
        {
            if (admin is null)
            {
                return Problem<UserData>(HttpStatusCode.BadRequest, "用户名或密码错误");
            }
            if (admin.Islock && !admin.IsSystem)
            {
                return Problem<UserData>(HttpStatusCode.Locked, "账号已锁定");
            }
            var result = DecryptStringObj(Password);
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
                        admin.Islock = true;
                        admin.ErrorCount = 0;
                        admin.LockTime = Clock.Now;
                        msg = "该账户已被锁定，请联系超级管理员解锁";
                    }
                    _unitOfWork.SetTransaction();
                    _unitOfWork.GetRepository<SysAdmin>().Update(admin);
                    await _unitOfWork.SaveChangesTranAsync();
                    return Problem<UserData>(HttpStatusCode.BadRequest, msg);
                }
                else
                {
                    return Problem<UserData>(HttpStatusCode.BadRequest, "用户名或密码错误");
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
                return Problem<UserData>(HttpStatusCode.BadRequest, "用户名或密码为空");
            var admin_unit = _unitOfWork.GetRepository<SysAdmin>();
            var Account = string.Empty;
            var Password = string.Empty;
            ProblemDetails<UserData> result = DecryptStringObj(adminLoginDto.Account);
            if (!result.IsSuccess)
            {
                return result;
            }
            Account = result.Detail;
            if (!admin_unit.Exists(m => m.Account == Account))
            {
                return Problem<UserData>(HttpStatusCode.BadRequest, "用户名或密码错误");
            }
            var admin = await admin_unit.GetFirstOrDefaultAsync(m => m.Account == Account, null, null, true, false);
            
            result = await CheckPasswordAsync(admin, adminLoginDto.Password).ConfigureAwait(false);
            if (!result.IsSuccess)
            {
                return result;
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
            return Problem(HttpStatusCode.OK, _mapper.Map<UserData>(admin));
        }
    }
}
