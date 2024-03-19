using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.EFSql.Transaction
{
    public class UnitOfWorkStatus
    {
        public bool IsStartingUow { get; internal set; }
    }
}
