using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.EFSqlServer.Transaction
{
    public class UnitOfWorkStatus
    {
        public bool IsStartingUow { get; internal set; }
    }
}
