using Flex.Application.Contracts.ISignalRBus.Enum;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.SignalRBus.Factory;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;

namespace Flex.Application.SignalRBus.Services
{
    public class TaskServices : ITaskServices
    {
        private IServiceProvider _serviceProvider;
        public TaskServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        // 添加任务到任务列表和队列
        public async Task AddTaskAsync(long userId, TaskModel<RequestModel> taskModel)
        {
            ExportAndImport.taskModels.AddOrUpdate(
                userId,
                id => [taskModel],
                (id, existingList) => existingList.Add(taskModel));

            var type = taskModel.TaskCate.GetType();
            if (TaskManager.requestDict.TryGetValue(type.Name, out var requestModel))
            {
                await ProcessRequestModelAsync(requestModel, taskModel.TaskCate);
            }
        }

        // 处理 RequestModel 的子类
        private async Task ProcessRequestModelAsync(RequestModel type, RequestModel requestModel)
        {
            switch (type)
            {
                case ExportRequestModel exportRequest:
                    await _serviceProvider.GetRequiredService<IConcurrentQueue<ExportRequestModel>>().EnqueueAsync((ExportRequestModel)requestModel);
                    break;
                case ImportRequestModel importRequest:
                    await _serviceProvider.GetRequiredService<IConcurrentQueue<ImportRequestModel>>().EnqueueAsync((ImportRequestModel)requestModel);
                    break;
                default:
                    throw new InvalidOperationException("Unknown RequestModel type");
            }
        }

        public ImmutableList<TaskModel<RequestModel>> GetTaskModelsAsync(long userId)
        {
            if (ExportAndImport.taskModels.TryGetValue(userId, out var taskList))
            {
                return taskList;
            }
            return ImmutableList<TaskModel<RequestModel>>.Empty; // 返回一个空的 ImmutableList
        }

        public void UpdateTaskStatus(RequestModel model, decimal Percent)
        {
            if (ExportAndImport.taskModels.TryGetValue(model.UserId, out var taskList))
            {
                // 使用 LINQ 查找要更新的任务，并创建一个新的列表
                var updatedTaskList = taskList.Select(task =>
                {
                    if (task.TaskId == model.CurrrentTaskId)
                    {
                        // 返回一个新的任务模型实例，包含更新后的状态
                        return new TaskModel<RequestModel>
                        {
                            TaskId = model.CurrrentTaskId,
                            TaskCate = task.TaskCate,
                            Percent = Percent,
                            Status = task.Status,
                            Name = task.Name,
                            Desc = task.Desc,
                            StatusString = task.StatusString,
                            AddTime = task.AddTime
                        };
                    }
                    return task;
                }).ToImmutableList(); // 创建一个新的不可变列表
                // 更新字典中的列表
                ExportAndImport.taskModels[model.UserId] = updatedTaskList;
            }
        }

        public void UpdateTaskStatus(RequestModel model, GlobalTaskStatus newStatus, string Desc = "", decimal Percent = -1)
        {
            if (ExportAndImport.taskModels.TryGetValue(model.UserId, out var taskList))
            {
                // 使用 LINQ 查找要更新的任务，并创建一个新的列表
                var updatedTaskList = taskList.Select(task =>
                {
                    if (task.TaskId == model.CurrrentTaskId)
                    {
                        // 返回一个新的任务模型实例，包含更新后的状态
                        return new TaskModel<RequestModel>
                        {
                            TaskId = model.CurrrentTaskId,
                            TaskCate = task.TaskCate,
                            Status = newStatus,
                            Name = task.Name,
                            Percent = Percent != -1 ? Percent : task.Percent,
                            Desc = Desc.IsNullOrEmpty() ? task.Desc : Desc,
                            StatusString = newStatus.GetEnumDescription(),
                            AddTime = task.AddTime
                        };
                    }
                    return task;
                }).ToImmutableList(); // 创建一个新的不可变列表

                // 更新字典中的列表
                ExportAndImport.taskModels[model.UserId] = updatedTaskList;
            }
        }


        // 删除任务
        public void RemoveTask(long userId, TaskModel<RequestModel> taskModel)
        {
            if (ExportAndImport.taskModels.TryGetValue(userId, out var existingList))
            {
                lock (existingList)
                {
                    existingList.Remove(taskModel);
                    if (existingList.Count == 0)
                    {
                        ExportAndImport.taskModels.TryRemove(userId, out _);
                    }
                }
            }
        }

        // 处理队列中的任务
        public async Task ProcessQueueAsync(Func<RequestModel, Task> processItemAsync, CancellationToken cancellationToken)
        {
            //await _taskQueue.ProcessQueueAsync(processItemAsync, cancellationToken);
        }
    }
}
