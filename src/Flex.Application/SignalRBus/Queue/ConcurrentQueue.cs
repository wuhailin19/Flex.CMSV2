using Flex.Application.Contracts.ISignalRBus.Enum;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.SignalRBus.Services;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Flex.Application.SignalRBus.Queue
{
    public class ConcurrentQueue<T> : IConcurrentQueue<T> where T : RequestModel
    {
        private readonly System.Collections.Concurrent.ConcurrentQueue<T> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);
        private IServiceProvider _serviceProvider;
        public ConcurrentQueue(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task EnqueueAsync(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));


            _serviceProvider.GetRequiredService<ITaskServices>().UpdateTaskStatus(item, GlobalTaskStatus.Waiting);
            // 添加任务到队列
            _queue.Enqueue(item);
            _signal.Release(); // 释放信号，表示有新请求
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

                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("接收到取消信号，停止处理队列。");
                    return; // 立即返回，结束处理
                }

                if (_queue.TryDequeue(out var item))
                {
                    await ExportAndImport._processingSemaphore.WaitAsync(cancellationToken); // 控制全局并发

                    try
                    {
                        await processItemAsync(item);
                    }
                    catch (Exception ex)
                    {
                        _serviceProvider.GetRequiredService<ITaskServices>().UpdateTaskStatus(item, GlobalTaskStatus.Ending, "出错");
                        throw;
                    }
                    finally
                    {
                        _serviceProvider.GetRequiredService<ITaskServices>().UpdateTaskStatus(item, GlobalTaskStatus.Ending);

                        ExportAndImport._processingSemaphore.Release(); // 释放信号
                    }
                }
            }
        }
    }
}
