using Flex.Application.Contracts.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[Authorize(AuthorizePolicy.Default)]
public abstract class ApiBaseController : ControllerBase
{
    /// <summary>
    /// 获取传递的Json并转换为T
    /// </summary>
    /// <returns></returns>
    public T GetModel<T>()
    {
        var sr = new StreamReader(Request.Body);
        var stream = sr.ReadToEnd();
        return JsonSerializer.Deserialize<T>(stream);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string Success(string message)
    {
        return Message<string>.Msg(ErrorCode.Success, null, message);
    }
    /// <summary>
    /// 返回成功
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static string Success<T>(T data = default, int code = ErrorCode.Success)
    {
        return Message<T>.Msg(code, data);
    }

    /// <summary>
    /// 返回失败
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static string Fail<T>(T msg = default, int code = ErrorCode.Fail)
    {
        return Message<T>.Msg(code, msg);
    }
    /// <summary>
    /// 返回失败
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static string Fail(string message)
    {
        return Message<string>.Msg(ErrorCode.Fail, null, message);
    }
    /// <summary>
    /// 返回未找到
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static string NotFound(int code = ErrorCode.NotFound)
    {
        return Message<string>.Msg(code, "Not Found");
    }
}
