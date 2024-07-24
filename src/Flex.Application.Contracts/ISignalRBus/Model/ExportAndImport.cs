using Flex.Domain.Dtos.SignalRBus.Model.Request;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Flex.Application.Contracts.ISignalRBus.Model
{
    public class ExportAndImport
    {
        public static bool _isProcessing = false; // 全局任务处理标志
        public static readonly SemaphoreSlim _processingSemaphore = new(1, 1); // 控制全局并发
        public static ConcurrentDictionary<long, ImmutableList<TaskModel<RequestModel>>> taskModels = new ConcurrentDictionary<long, ImmutableList<TaskModel<RequestModel>>>();
    }
}
