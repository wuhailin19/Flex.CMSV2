using Castle.DynamicProxy;
using Flex.Application.Contracts.Aop;
using Flex.Core.Attributes;
using Flex.Core.Serialize;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Flex.Application.Aop
{
    public class LogInterceptorAsync : IAsyncInterceptor
    {
        private readonly ILogger<LogInterceptorAsync> _logger;
        private readonly IWebHostEnvironment _env;
        public LogInterceptorAsync(ILogger<LogInterceptorAsync> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// 同步方法拦截时使用
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            if ((invocation.Method?.IsDefined(typeof(NoLogAttribute), true) ?? false) ||
                 (invocation.Method.DeclaringType?.IsDefined(typeof(NoLogAttribute), true) ?? false))
            {
                invocation.Proceed();
                return;
            }
            try
            {
                //调用业务方法
                invocation.Proceed();
                if ((invocation.Method?.IsDefined(typeof(NoLogReturnValueAttribute), true) ?? false) ||
               (invocation.Method.DeclaringType?.IsDefined(typeof(NoLogReturnValueAttribute), true) ?? false))
                {
                    LogExecuteInfo(invocation, "返回值过长，不予记录");
                }
                else
                {
                    LogExecuteInfo(invocation, JsonHelper.ToJson(invocation.ReturnValue, NamingType.CamelCase, false));//记录日志
                }
            }
            catch (AopHandledException ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new AopHandledException(ex.ExceptionTitle, msg, ex);
            }
            catch (WarningHandledException ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new WarningHandledException(ex.ExceptionTitle, msg, ex);
            }
            catch (Exception ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new AopHandledException(string.Empty, msg, ex);
            }
        }

        /// <summary>
        /// 异步方法返回Task时使用
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous(IInvocation invocation)
        {
            if ((invocation.Method?.IsDefined(typeof(NoLogAttribute), true) ?? false) ||
                 (invocation.Method.DeclaringType?.IsDefined(typeof(NoLogAttribute), true) ?? false))
            {
                invocation.Proceed();
                return;
            }
            try
            {
                //调用业务方法
                invocation.Proceed();
                if ((invocation.Method?.IsDefined(typeof(NoLogReturnValueAttribute), true) ?? false) ||
               (invocation.Method.DeclaringType?.IsDefined(typeof(NoLogReturnValueAttribute), true) ?? false))
                {
                    LogExecuteInfo(invocation, "返回值过长，不予记录");
                }
                else
                {
                    LogExecuteInfo(invocation, JsonHelper.ToJson(invocation.ReturnValue, NamingType.CamelCase, false));//记录日志
                }
            }
            catch (AopHandledException ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new AopHandledException(ex.ExceptionTitle, msg, ex);
            }
            catch (WarningHandledException ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new WarningHandledException(ex.ExceptionTitle, msg, ex);
            }
            catch (Exception ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new AopHandledException(string.Empty, msg, ex);
            }
        }

        /// <summary>
        /// 异步方法返回Task<T></T>时使用
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        /// </summary>
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            //调用业务方法
            invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
        }
        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            if ((invocation.Method?.IsDefined(typeof(NoLogAttribute), true) ?? false) ||
                (invocation.Method.DeclaringType?.IsDefined(typeof(NoLogAttribute), true) ?? false))
            {
                //调用业务方法
                invocation.Proceed();
                Task<TResult> task = (Task<TResult>)invocation.ReturnValue;
                TResult result = await task;//获得返回结果
                return result;
            }
            try
            {
                //调用业务方法
                invocation.Proceed();
                Task<TResult> task = (Task<TResult>)invocation.ReturnValue;
                TResult result = await task;//获得返回结果
                if ((invocation.Method?.IsDefined(typeof(NoLogReturnValueAttribute), true) ?? false) ||
                (invocation.Method.DeclaringType?.IsDefined(typeof(NoLogReturnValueAttribute), true) ?? false))
                {
                    LogExecuteInfo(invocation, "返回值过长，不予记录");
                }
                else
                { 
                    LogExecuteInfo(invocation, JsonHelper.ToJson(result, NamingType.CamelCase, false));
                }
                return result;
            }
            catch (AopHandledException ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new AopHandledException(ex.ExceptionTitle, msg, ex);
            }
            catch (WarningHandledException ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new WarningHandledException(ex.ExceptionTitle, msg, ex);
            }
            catch (Exception ex)
            {
                LogExecuteError(ex, invocation, out string msg);
                throw new AopHandledException(string.Empty, msg, ex);
            }
        }

        #region helpMethod
        /// <summary>
        /// 获取拦截方法信息（类名、方法名、参数）
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private string GetMethodInfo(IInvocation invocation)
        {
            //方法类名
            string className = invocation.Method.DeclaringType.Name;
            //方法名
            string methodName = invocation.Method.Name;
            //参数
            string args = string.Empty;
            if (invocation.Arguments.GetType().IsSimpleType())
                args = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());
            else
                args = JsonHelper.ToJson(invocation.Arguments, NamingType.CamelCase, false);

            if (string.IsNullOrWhiteSpace(args))
                return $"{className} {methodName}";
            else
                return $"{className} {methodName} {args}";
        }
        private void LogExecuteInfo(IInvocation invocation, string result)
        {
            //Console.WriteLine("方法{0}，返回值{1}", GetMethodInfo(invocation), result);
            _logger.LogInformation("{0} {1}", GetMethodInfo(invocation), result);
        }
        private void LogExecuteError(Exception ex, IInvocation invocation, out string msg)
        {
            //Console.WriteLine(ex.Message, "执行{0}时发生错误！", GetMethodInfo(invocation));
            msg = _env.IsDevelopment() ? GetMethodInfo(invocation) : "操作失败，请根据事件ID进行错误追踪";
            //_logger.Log(LogLevel.Error, "执行{0}时发生错误！", string.Empty);
        }
        #endregion
    }
}
