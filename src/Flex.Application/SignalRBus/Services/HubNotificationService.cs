using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Application.SignalRBus.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Flex.Application.SignalRBus.Services
{
    public class HubNotificationService : IHubNotificationService
    {
        private readonly IHubContext<ExportHub> _hubContext;
        public HubNotificationService(IHubContext<ExportHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public ConnectionModel GetConnectionModel(long userId)
        {
            if (ExportHub.UserConnections.TryGetValue(userId, out var connectionModel))
            {
                return connectionModel;
            }
            return null;
        }
        public bool ValidateUser(long userId)
        {
            var connectionModel = GetConnectionModel(userId);
            if (connectionModel != null)
            {
                if (connectionModel.ExpiryTime > DateTime.UtcNow)
                {
                    // 用户存在且未过期
                    return true;
                }
                else
                {
                    // 连接已过期，移除用户
                    RemoveUser(userId);
                }
            }
            // 用户不存在或连接已过期
            return false;
        }

        /// <summary>
        /// 让用户下线
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveUser(long userId)
        {
            ExportHub.UserConnections.TryRemove(userId, out var removedModel);
            if (removedModel != null)
            {
                foreach (var connectionId in removedModel.ConnectionId)
                {
                    // 这里可以执行额外的清理操作，例如通知其他组件用户下线等
                }
            }
        }


        public ConnectionModel GetClaims(long userId)
        {
            var model = ExportHub.GetConnectionModel(userId);
            return model != null ? model : null;
        }

        public async Task ReceiveMessage(long userId, string progress)
        {
            var connectionId = ExportHub.GetConnectionId(userId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", progress);
            }
        }

        public async Task SendError(long userId, string error)
        {
            var connectionId = ExportHub.GetConnectionId(userId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ExportError", error);
            }
        }

        public async Task NotifyCompletion(long userId, string message)
        {
            var connectionId = ExportHub.GetConnectionId(userId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ExportCompleted", message);
            }
        }

        public async Task SendProgress(long userId, string message)
        {
            var connectionId = ExportHub.GetConnectionId(userId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("SendProgress", message);
            }
        }

        public async Task SendNotificationToUser(long userId, string message)
        {
            var connectionId = ExportHub.GetConnectionId(userId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("SendNotificationToUser", message);
            }
        }

        public async Task SendNotificationToRole(string role, string message)
        {
            await _hubContext.Clients.Group(role).SendAsync("SendNotificationToRole", message);
        }
    }

}
