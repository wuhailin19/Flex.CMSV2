using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Flex.Application.SignalRBus.Hubs
{
    public class ExportHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();
        public void RegisterUser(string userId)
        {
            if (Context.User.Identity?.IsAuthenticated ?? false)
            {
                var connectionId = Context.ConnectionId;
                UserConnections[userId] = connectionId;
            }
            else
            {
                // 用户未通过身份验证，拒绝连接
                Context.Abort();
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var item = UserConnections.FirstOrDefault(k => k.Value == Context.ConnectionId);
            if (item.Key != null)
            {
                UserConnections.TryRemove(item.Key, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public static string GetConnectionId(string userId)
        {
            return UserConnections.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }

        public async Task SendProgress(string connectionId, string progress)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", progress);
        }

        public async Task NotifyCompletion(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ExportCompleted", message);
        }
    }
}
