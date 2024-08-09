using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Core.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using SqlSugar.Extensions;
using System.Collections.Concurrent;

namespace Flex.Application.SignalRBus.Hubs
{

	public class UserHub : Hub
	{
		IClaimsAccessor _claims;
		ICaching _caching;
		public static readonly ConcurrentDictionary<long, ConnectionModel> UserConnections = new ConcurrentDictionary<long, ConnectionModel>();
		public UserHub(
			IClaimsAccessor claims
			, ICaching caching
			)
		{
			_claims = claims;
			_caching = caching;
		}


		//这里每次刷新主页都会重置一次当前链接用户的在线信息和剩余时间
		public override async Task OnConnectedAsync()
		{
			if (Context.User.Identity?.IsAuthenticated ?? false)
			{
				// 从缓存中获取 token 的有效期
				var tokenKey = Context.GetHttpContext().Request.Query["access_token"].ToString().Replace($"{JwtBearerDefaults.AuthenticationScheme} ", string.Empty);
				var tokencachevalue = _caching.Get(tokenKey);
				var tokenexpiryTime = tokencachevalue.ObjToDate();
				if (tokenexpiryTime == DateTime.MinValue || tokenexpiryTime.ObjToDate() < Clock.Now)
				{
					// Token 已过期，拒绝连接
					Context.Abort();
					return;
				}

				var connectionId = Context.ConnectionId;

				var model = new ConnectionModel
				{
					UserId = _claims.UserId,
					UserName = _claims.UserName,
					ConnectionId = connectionId,
					Token = tokenKey,
					Status = true
				};

				UserConnections.AddOrUpdate(model.UserId, model, (k, value) => model);

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
			var connectionId = Context.ConnectionId;
			var userConnection = UserConnections.FirstOrDefault(c => c.Value.ConnectionId == connectionId);
			if (userConnection.Value != null)
			{
				UserConnections.TryRemove(userConnection.Key, out _);
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
