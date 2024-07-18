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

        public ConnectionModel GetClaims(long userId) {
           var model= ExportHub.GetConnectionModel(userId);
            return model != null ? model : null;
        }

        public async Task SendProgress(long userId, string progress)
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
