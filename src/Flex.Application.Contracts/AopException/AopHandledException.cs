using Flex.Application.Contracts.Exceptions;
using Flex.Core.Extensions;

namespace Flex.Application.Contracts.Aop
{
    /// <summary>
    /// 使用自定义的Exception，用于在aop中已经处理过的异常，在其他地方不用重复记录日志
    /// </summary>
    public class AopHandledException : ApplicationException
    {
        public string ErrorMessage { get; private set; }
        public string ExceptionTitle { get; private set; } = "发生异常";
        public Exception InnerHandledException { get; private set; }
        //无参数构造函数
        public AopHandledException()
        {

        }
        //带一个字符串参数的构造函数，作用：当程序员用Exception类获取异常信息而非 MyException时把自定义异常信息传递过去
        public AopHandledException(string msg) : base(msg)
        {
            this.ErrorMessage = msg;
        }
        public AopHandledException(string exceptiontitle, Exception ex)
        {
            if (exceptiontitle.IsNotNullOrEmpty())
                this.ExceptionTitle = exceptiontitle;
            this.InnerHandledException = ex;
        }
        //带有一个字符串参数和一个内部异常信息参数的构造函数
        public AopHandledException(string exceptiontitle, string msg, Exception innerException) : base(msg)
        {
            if (exceptiontitle.IsNotNullOrEmpty())
                this.ExceptionTitle = exceptiontitle;
            this.InnerHandledException = innerException;
            this.ErrorMessage = msg;
        }
        //带有一个字符串参数和一个内部异常信息参数的构造函数
        public AopHandledException(ErrorCodes statuscode, Exception innerException)
        {
            this.InnerHandledException = innerException;
            this.ErrorMessage = statuscode.GetEnumDescription();
        }
        public string GetError()
        {
            return ErrorMessage;
        }
    }
}
