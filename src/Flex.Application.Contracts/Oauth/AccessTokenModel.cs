using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.Oauth
{
    public class AccessTokenModel
    {
        public string access_token { set; get; }
        public string expires_in { set; get; }
        public string remind_in { set; get; }
        public string uid { set; get; }
    }
}
