using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Flex.Application.SignalRBus.Hubs
{
    public class ExportHub : Hub
    {
        private readonly IClaimsAccessor _claims;
        private static readonly ConcurrentDictionary<long, string> UserConnections = new ConcurrentDictionary<long, string>();
        public ExportHub(IClaimsAccessor claims)
        {
            _claims = claims;
        }
        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity?.IsAuthenticated ?? false)
            {
                var connectionId = Context.ConnectionId;
                var Claims = _claims.UserRole;
                UserConnections[_claims.UserId] = connectionId;

                // 将用户加入相应的角色组
                await Groups.AddToGroupAsync(connectionId, _claims.UserRole.ToString());
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

        public static string GetConnectionId(long userId)
        {
            return UserConnections.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }
        /// <summary>
        /// 按用户发信
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendNotificationToUser(long userId, string message)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }
        /// <summary>
        /// 按角色发信
        /// </summary>
        /// <param name="role"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendNotificationToRole(string role, string message)
        {
            await Clients.Group(role).SendAsync("ReceiveNotification", message);
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
