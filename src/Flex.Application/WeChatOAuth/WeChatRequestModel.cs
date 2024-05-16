using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.WeChatOAuth
{
    public class WeChatRequestModel
    {
        public string signature { set; get; }
        public string timestamp { set; get; }
        public string nonce { set; get; }
        public string echostr { set; get; }
    }
}
