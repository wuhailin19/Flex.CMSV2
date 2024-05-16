using Flex.Application.WeChatOAuth;
using Flex.Core.Attributes;
using Flex.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Flex.WebApi.SystemControllers
{
    [Route("api/[controller]")]
    [Descriper(Name = "微信相关接口", IsFilter = true)]
    public class WeiChatController : ApiBaseController
    {
        private ILogger<WeiChatController> _logger;
        public WeiChatController(ILogger<WeiChatController> logger) {
            _logger = logger;
        }
        
        /// <summary>
        /// 定义Token，与微信公共平台上的Token保持一致
        /// </summary>
        private const string Token = "flexcmsToken";
        [HttpGet("ValidGet")]
        [AllowAnonymous]
        public string ValidGet(WeChatRequestModel model)
        {
            _logger.LogInformation(JsonHelper.ToJson(model));
            //获取请求来的 echostr 参数
            string echoStr = model.echostr;
            //通过验证，出于安全考虑。（也可以跳过）
            if (CheckSignature(model))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    _logger.LogInformation("成功："+ echoStr);
                    return echoStr;
                }
            }
            _logger.LogInformation("验证失败");
            return string.Empty;
        }
        /// <summary>
        /// 验证签名，检验是否是从微信服务器上发出的请求
        /// </summary>
        /// <param name="model">请求参数模型 Model</param>
        /// <returns>是否验证通过</returns>
        private bool CheckSignature(WeChatRequestModel model)
        {
            string signature, timestamp, nonce, tempStr;
            //获取请求来的参数
            signature = model.signature;
            timestamp = model.timestamp;
            nonce = model.nonce;
            //创建数组，将 Token, timestamp, nonce 三个参数加入数组
            string[] array = { Token, timestamp, nonce };
            //进行排序
            Array.Sort(array);
            //拼接为一个字符串
            tempStr = String.Join("", array);
            //对字符串进行 SHA1加密
            tempStr = SHA1_Encrypt(tempStr);
            //判断signature 是否正确
       
            if (tempStr.Equals(signature,StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string Get_SHA1_Method2(string strSource)
        {
            string strResult = "";
            //Create 
            System.Security.Cryptography.SHA1 md5 = System.Security.Cryptography.SHA1.Create();
            //注意编码UTF8、UTF7、Unicode等的选择 
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strSource));
            //字节类型的数组转换为字符串 
            for (int i = 0; i < bytResult.Length; i++)
            {
                //16进制转换 
                strResult = strResult + bytResult[i].ToString("X");
            }
            return strResult.ToLower();
        }

        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="strIN">需要加密的字符串</param>
        /// <returns>密文</returns>
        private static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            System.Security.Cryptography.SHA1 iSHA = System.Security.Cryptography.SHA1.Create();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString().ToUpper();
        }

    }
}
