using Flex.Application.Contracts.Aop;
using Flex.Core.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Flex.Application.Contracts.Exceptions
{
    public class FillExceptionModel
    {
        public string title { set; get; }
        public string detial { set; get; }
        public LogLevel logLevel { set; get; } = LogLevel.Error;
        public FillExceptionModel() { }

        public FillExceptionModel(bool IsDevelopment, Exception exception, string requestId)
        {
            title = IsDevelopment ? exception.Message : $"系统异常";
            detial = IsDevelopment ? exception.Format() : $"系统异常,请联系管理员({requestId})";

            if (exception is AopHandledException)
            {
                var aopex = (AopHandledException)exception;
                if (aopex.InnerHandledException is OverflowException)
                {
                    title = "输入异常";
                    detial = "值超过最大限度";
                }
                else
                {
                    title = aopex.ExceptionTitle;
                    detial = $"请联系管理员处理，本次故障Id：{requestId}";
                }
            }
            else if (exception is WarningHandledException)
            {
                var aopex = (WarningHandledException)exception;
                title = aopex.ExceptionTitle;
                detial = aopex.ErrorMessage;
                logLevel = LogLevel.Warning;
            }
        }
    }
}
