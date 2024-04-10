namespace Flex.Application.Contracts.Aop
{
    /// <summary>
    /// 用于警告级别日志记录
    /// </summary>
    public class WarningHandledException : ApplicationException
    {
        public string ErrorMessage { get; private set; }
        public Exception InnerHandledException { get; private set; }
        //无参数构造函数
        public WarningHandledException()
        {

        }
        //带一个字符串参数的构造函数，作用：当程序员用Exception类获取异常信息而非 MyException时把自定义异常信息传递过去
        public WarningHandledException(string msg) : base(msg)
        {
            this.ErrorMessage = msg;
        }
    }
}
