using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Application.Extensions.Register.WebCoreExtensions
{
    public class SiteSetting
    {
        public long WorkerId { get; set; }
        public long DataCenterId { get; set; }
        public int LoginFailedCountLimits { get; set; }
        public int LoginLockedTimeout { get; set; }
    }
}
