using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.ISignalRBus.Model
{
    public class ConnectionModel
    {
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
