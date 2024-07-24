using Flex.Application.Contracts.ISignalRBus.Enum;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using System.Collections.Immutable;

namespace Flex.Application.Contracts.ISignalRBus.IServices
{
    public interface ITaskServices
    {
        [NoLogReturnValue]
        Task AddTaskAsync(long userId, TaskModel<RequestModel> taskModel);
        ImmutableList<TaskModel<RequestModel>> GetTaskModelsAsync(long userId);
        Task ProcessQueueAsync(Func<RequestModel, Task> processItemAsync, CancellationToken cancellationToken);
        void RemoveTask(long userId, TaskModel<RequestModel> taskModel);
        void UpdateTaskStatus(RequestModel model, GlobalTaskStatus newStatus, string Desc = "", decimal Percent = -1);
        void UpdateTaskStatus(RequestModel model, decimal Percent);
    }
}
