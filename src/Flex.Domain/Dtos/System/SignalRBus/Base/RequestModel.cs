using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.SignalRBus.Model.Request
{
    public class RequestModel
    {
        public long CurrrentTaskId { set; get; }
        public long UserId { set; get; }
    }
}
