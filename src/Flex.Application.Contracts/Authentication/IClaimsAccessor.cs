﻿using Flex.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Application.Authorize
{
    [NoLog]
    public interface IClaimsAccessor
    {
        string UserName { get; }
        long UserId { get; }
        string UserAccount { get; }
        int UserRole { get; }
        string UserRoleDisplayName { get; }
        bool IsSystem { get; }
        bool IsAuthenticated { get; }
    }
}
