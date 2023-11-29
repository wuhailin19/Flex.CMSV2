using Flex.Core.Helper;
/// <summary>
/// 返回信息
/// </summary>
namespace Flex.Core
{
    public class Message<T> : IMessage<T>
    {
        public int code { get; set; }
        public T content { get; set; }
        public string msg { get; set; }

        /// <summary>
        /// 输出对应的信息返回至前端
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statecode"></param>
        /// <param name="message"></param>
        public static string Msg(int statecode, T data, string message=null)
        {
            Message<T> messages = new Message<T>();
            messages.code = statecode;
            messages.content = data;
            messages.msg = message;
            string str = JsonHelper.ToJson(messages);
            return str;
        }
    }
}