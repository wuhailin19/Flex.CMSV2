using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.Exceptions
{
    public class ExceptionMsg
    {
        public int code { get; set; }
        public string EventId { get; set; }
        public string msg { get; set; }
        public string Instance { get; set; }
        public string Type { get; set; }
        public string title { get; set; }
    }
}
