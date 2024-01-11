﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Dapper
{
    public interface IContext : IDisposable
    {
        bool IsTransactionStarted { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
