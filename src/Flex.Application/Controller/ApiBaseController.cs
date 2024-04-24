using Flex.Application.Contracts.Authorize;
using Flex.Domain.Dtos.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

[Authorize(AuthorizePolicy.Default)]
public abstract class ApiBaseController : ControllerBase
{
    /// <summary>
    /// 【不验证】获取传递的Json并转换为T
    /// </summary>
    /// <returns></returns>
    public async Task<T> GetModel<T>()
    {
        using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
        var stream = await reader.ReadToEndAsync();

        return JsonHelper.Json<T>(stream);
    }
    /// <summary>
    /// 【验证】传递的Json并转换为T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<ProblemDetails<T>> ValidateModel<T>()
    {
        // 使用验证器验证实体
        var model = await GetModel<T>();
        if (model is null)
            return new ProblemDetails<T>(HttpStatusCode.BadRequest, "验证未通过");
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);
        if (!isValid)
        {
            return new ProblemDetails<T>(HttpStatusCode.BadRequest, validationResults[0]?.ErrorMessage ?? "验证未通过");
        }
        return new ProblemDetails<T>(HttpStatusCode.OK, model, "验证通过");
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
    /// <param name="data">泛型</param>
    /// <param name="code"></param>
    /// <returns></returns>
    public static string Success<T>(T data = default, int code = ErrorCode.Success)
    {
        return Message<T>.Msg(code, data);
    }

    /// <summary>
    /// 返回失败
    /// </summary>
    /// <param name="msg">消息</param>
    /// <param name="code"></param>
    /// <returns></returns>
    public static string Fail<T>(T msg = default, int code = ErrorCode.Fail)
    {
        return Message<T>.Msg(code, msg);
    }
    /// <summary>
    /// 返回失败
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns></returns>
    public static string Fail(string message)
    {
        return Message<string>.Msg(ErrorCode.Fail, null, message);
    }
    /// <summary>
    /// 返回失败，带消息
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="code"></param>
    /// <returns></returns>
    public static string Fail(string message, int code)
    {
        return Message<string>.Msg(code, null, message);
    }
    /// <summary>
    /// 返回未找到
    /// </summary>
    /// <param name="code">404</param>
    /// <returns></returns>
    public static string NotFound(int code = ErrorCode.NotFound)
    {
        return Message<string>.Msg(code, "Not Found");
    }
}
