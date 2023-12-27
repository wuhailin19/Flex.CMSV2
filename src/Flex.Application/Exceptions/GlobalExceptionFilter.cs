using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Flex.Application.Exceptions
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IWebHostEnvironment _env;
        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
            _env = env;
        }
        public override void OnException(ExceptionContext context)
        {
            var status = 500;
            var exception = context.Exception;
            var requestId = System.Diagnostics.Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            //var eventId = new Microsoft.Extensions.Logging.EventId(exception.HResult, requestId);
            var hostAndPort = context.HttpContext.Request.Host.HasValue ? context.HttpContext.Request.Host.Value : string.Empty;
            var requestUrl = string.Concat(hostAndPort, context.HttpContext.Request.Path);
            var type = string.Concat("https://httpstatuses.com/", status);
            string title;
            string detial;
            if (exception is BusiException)
            {
                title = "参数错误";
                detial = exception.Message;
            }
            else
            {
                title = _env.IsDevelopment() ? exception.Message : $"系统异常";
                detial = _env.IsDevelopment() ? exception.Format() : $"系统异常,请联系管理员({requestId})";
                var exlog = new ExceptionLog()
                {
                    RequestUrl = requestUrl,
                    EventId = requestId,
                    Exception = exception
                };
                _logger.Log(LogLevel.Error, JsonHelper.ToJson(exlog));
            }
            var problemDetails = new ExceptionMsg
            {
                code = status,
                EventId = requestId,
                title = title,
                msg = detial,
                Type = type,
                Instance = requestUrl
            };
            context.Result = new ObjectResult(new Message<ExceptionMsg> { code = status, content = problemDetails }) { StatusCode = status };
            context.ExceptionHandled = true;
        }
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
