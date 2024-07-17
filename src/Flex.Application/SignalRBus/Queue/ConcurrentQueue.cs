using Flex.Application.Contracts.ISignalRBus.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.SignalRBus.Queue
{
    public class ConcurrentQueue<T> : IConcurrentQueue<T>
    {
        private readonly System.Collections.Concurrent.ConcurrentQueue<T> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);

        public Task EnqueueAsync(T item)
        {
            _queue.Enqueue(item);
            _signal.Release(); // 释放信号，表示有新请求
            return Task.CompletedTask;
        }

        public async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken); // 等待信号
            _queue.TryDequeue(out var result);
            return result;
        }
    }
}
