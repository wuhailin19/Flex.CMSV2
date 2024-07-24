using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.ISignalRBus.Queue
{
    public interface IConcurrentQueue<T>
    {
        Task EnqueueAsync(T item);
        Task<T> DequeueAsync(CancellationToken cancellationToken);
        Task ProcessQueueAsync(Func<T, Task> processItemAsync, CancellationToken cancellationToken);
    }
}
