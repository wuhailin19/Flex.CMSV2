namespace Flex.Application.Contracts.Aop
{
    /// <summary>
    /// 用于警告级别日志记录
    /// </summary>
    public class WarningHandledException : ApplicationException
    {
        public string ErrorMessage { get; private set; }
        public string ExceptionTitle { get; private set; }
        public Exception InnerHandledException { get; private set; }
        //无参数构造函数
        public WarningHandledException()
        {

        }
        //带一个字符串参数的构造函数，作用：当程序员用Exception类获取异常信息而非 MyException时把自定义异常信息传递过去
        public WarningHandledException(string msg)
        {
            this.ExceptionTitle = msg;
        }
        public WarningHandledException(string exceptiontitle, Exception ex)
        {
            this.ExceptionTitle = exceptiontitle;
            this.InnerHandledException = ex;
        }
        public WarningHandledException(string exceptiontitle,string msg, Exception ex)
        {
            this.ExceptionTitle = exceptiontitle;
            this.ErrorMessage = msg;
            this.InnerHandledException = ex;
        }
    }
}
