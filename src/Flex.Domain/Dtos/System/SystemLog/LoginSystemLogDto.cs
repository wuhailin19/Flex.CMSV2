using Flex.Domain.Enums.LogLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.SystemLog
{
    public class LoginSystemLogDto
    {
       public SystemLogLevel systemLogLevel { set; get; }
        public string operationContent { set; get; }
        public string inoperator { set; get; } = "";
        public bool IsAuthenticated { set; get; } = false;
        public long? UserId { set; get; }
        public string? UserName { set; get; }
    }
}
