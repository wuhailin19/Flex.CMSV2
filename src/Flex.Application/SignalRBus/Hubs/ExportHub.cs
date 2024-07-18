using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Flex.Application.Contracts.ISignalRBus.Model;
using Microsoft.AspNetCore.SignalR;

namespace Flex.Application.SignalRBus.Hubs
{
    
    public class ExportHub : Hub
    {
        IClaimsAccessor _claims;
        private static readonly ConcurrentDictionary<long, ConnectionModel> UserConnections = new ConcurrentDictionary<long, ConnectionModel>();
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
                UserConnections[_claims.UserId] = new ConnectionModel { 
                    UserId = _claims.UserId, 
                    UserName = _claims.UserName, 
                    ConnectionId = connectionId };

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
            var item = UserConnections.FirstOrDefault(k => k.Value.ConnectionId == Context.ConnectionId);
            if (item.Key != null)
            {
                UserConnections.TryRemove(item.Key, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public static string GetConnectionId(long userId)
        {
            var model = GetConnectionModel(userId);
            return model != null ? model.ConnectionId : null;
        }

        public static ConnectionModel GetConnectionModel(long userId)
        {
            return UserConnections.TryGetValue(userId, out var value) ? value : null;
        }
    }
}
