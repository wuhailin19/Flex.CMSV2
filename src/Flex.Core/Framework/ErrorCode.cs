using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// ErrorCode 的摘要说明
/// </summary>
public class ErrorCode
{
    /// <summary>
    /// 参数为空
    /// </summary>
    [Description("参数为空")]
    public const int ParamEmpty = 402;
    /// <summary>
    /// 成功
    /// </summary>
    public const int Success = 200;
    /// <summary>
    /// 资源未找到
    /// </summary>
    public const int NotFound = 404;
    /// <summary>
    /// 失败
    /// </summary>
    public const int Fail = 400;
    /// <summary>
    /// 错误
    /// </summary>
    public const int Error = 500;
    /// <summary>
    /// 没有权限
    /// </summary>
    public const int NotAuth = 403;
    /// <summary>
    /// 需要授权
    /// </summary>
    public const int AuthRequire = 401;
    /// <summary>
    /// 已锁定
    /// </summary>
    public const int IsLock = 405;
    /// <summary>
    /// 密码错误
    /// </summary>
    public const int PwdWrong = 406;
    /// <summary>
    /// 超时
    /// </summary>
    public const int Timeout = 303;
    /// <summary>
    /// Ip验证未通过
    /// </summary>
    public const int IpAuthFail = 304;
    /// <summary>
    /// 多人登录码错误
    /// </summary>
    public const int MulitcodeWrong = 305;
    /// <summary>
    /// 超管
    /// </summary>
    public const int SystemCode = 7888;
}