using System;

namespace Flex.Core.Timing
{
    public interface IClockProvider
    {
        DateTime Now { get; }
        DateTime Normalize(DateTime dateTime);
    }
}
