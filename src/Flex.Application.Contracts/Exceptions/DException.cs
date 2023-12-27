using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.Exceptions
{
    public class DException : System.Exception
    {
        public int Code { get; }

        public DException(string message, int code = ErrorCodes.DefaultCode)
            : base(message)
        {
            Code = code;
        }
    }
}
