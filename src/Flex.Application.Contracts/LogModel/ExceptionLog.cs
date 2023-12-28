using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.Logs
{
    public class ExceptionLog
    {
        public string RequestUrl { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string UserId { get; set; }
        public string EventId { get; set; }
        public System.Exception Exception { get; set; }
    }
}
