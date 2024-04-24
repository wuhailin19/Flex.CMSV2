using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices.System;
using Flex.Application.Contracts.Logs;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Enums.LogLevel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Flex.Application.Exceptions
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IClaimsAccessor _claimsAccessor;
        private ISystemLogServices _logServices;
        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger, IClaimsAccessor claimsAccessor,
            ISystemLogServices logServices)
        {
            _logger = logger;
            _env = env;
            _claimsAccessor = claimsAccessor;
            _logServices = logServices;
        }
        public override void OnException(ExceptionContext context)
        {
            var status = 206;
            var exception = context.Exception;
            var requestId = System.Diagnostics.Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            //var eventId = new Microsoft.Extensions.Logging.EventId(exception.HResult, requestId);
            var hostAndPort = context.HttpContext.Request.Host.HasValue ? context.HttpContext.Request.Host.Value : string.Empty;
            var requestUrl = string.Concat(hostAndPort, context.HttpContext.Request.Path);
            var type = string.Concat("https://httpstatuses.com/", status);

            var exceptionmodel = new FillExceptionModel(_env.IsDevelopment(), exception, requestId);

            var userid = "用户未认证";
            if (_claimsAccessor.IsAuthenticated)
            {
                userid = _claimsAccessor?.UserId.ToString() ?? "用户未认证";
            }
            var ActionAndController = context.ActionDescriptor as ControllerActionDescriptor;
            var exlog = new ExceptionLog()
            {
                RequestUrl = requestUrl,
                EventId = requestId,
                UserId = userid,
                ControllerName = ActionAndController?.ControllerName ?? string.Empty,
                ActionName = ActionAndController?.ActionName ?? string.Empty,
                Exception = exception
            };
            _logger.Log(exceptionmodel.logLevel, JsonHelper.ToJson(exlog));

            if (exceptionmodel.logLevel == LogLevel.Warning || exceptionmodel.logLevel == LogLevel.Error)
            {
                var logmodel = new InputSystemLogDto();
                logmodel.LogLevel = exceptionmodel.logLevel == LogLevel.Warning ? SystemLogLevel.Warning : SystemLogLevel.Error;
                logmodel.OperationContent = $"{exceptionmodel.title}，请求Id为：{requestId}，请到日志文件内查看详细记录";
                logmodel.LogSort = LogSort.Api;
                logmodel.Url = requestUrl;
                try
                {
                    _logServices.AddLog(logmodel).GetAwaiter().GetResult();
                }
                catch (AggregateException ex)
                {
                    _logger.Log(LogLevel.Error, "日志记录失败：" + ex.InnerException?.Message ?? string.Empty);
                }
            }
            var problemDetails = new ExceptionMsg
            {
                code = status,
                EventId = requestId,
                title = exceptionmodel.title,
                msg = exceptionmodel.detial,
                Type = type,
                Instance = requestUrl
            };
            context.Result = new ObjectResult(new Message<ExceptionMsg> { code = status, content = problemDetails, msg = exceptionmodel.title }) { StatusCode = status };
            context.ExceptionHandled = true;
        }
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
