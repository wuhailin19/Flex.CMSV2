﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Core.IDCode
{
    public class DisposableAction : IDisposable
    {
        readonly Action _action;

        public DisposableAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
