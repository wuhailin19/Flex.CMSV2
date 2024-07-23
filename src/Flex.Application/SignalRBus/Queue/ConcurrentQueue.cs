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
        private readonly SemaphoreSlim _processingSemaphore = new(1, 1); // 控制并发
        private bool _isProcessing = false; // 用于指示当前是否有任务在处理

        public Task EnqueueAsync(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

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

        public async Task ProcessQueueAsync(Func<T, Task> processItemAsync, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _signal.WaitAsync(cancellationToken); // 等待信号
                if (_queue.TryDequeue(out var item))
                {
                    await _processingSemaphore.WaitAsync(cancellationToken); // 控制并发
                    _isProcessing = true; // 设置任务处理中标志
                    try
                    {
                        await processItemAsync(item);
                    }
                    finally
                    {
                        _isProcessing = false; // 重置任务处理中标志
                        _processingSemaphore.Release(); // 释放信号
                    }
                }
            }
        }

        public bool IsProcessing()
        {
            return _isProcessing;
        }
    }
}
