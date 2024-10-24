using Flex.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Config
{
    public class OAuthLoginConfig
    {
        public static string AuthUrl => "OAuthLogin:AuthUrl".Config(string.Empty);
        public static string Redirect_Uri => "OAuthLogin:Redirect_Uri".Config(string.Empty);
        public static string AppKey => "OAuthLogin:Weibo:AppKey".Config(string.Empty);
        public static string AppSecret => "OAuthLogin:Weibo:AppSecret".Config(string.Empty);
    }
}
