using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Application.SignalRBus.Hubs;
using Flex.Core.Config;
using Microsoft.AspNetCore.SignalR;

namespace Flex.Application.SignalRBus.Services
{
	public class HubNotificationService : IHubNotificationService
	{
		private readonly IHubContext<UserHub> _hubContext;
		ICaching _caching;

		public HubNotificationService(
			IHubContext<UserHub> hubContext
			, ICaching caching)
		{
			_hubContext = hubContext;
			_caching = caching;
		}

		public ConnectionModel GetConnectionModel(long userId)
		{
			if (UserHub.UserConnections.TryGetValue(userId, out var connectionModel))
			{
				return connectionModel;
			}
			return null;
		}

		/// <summary>
		/// 让用户下线
		/// </summary>
		/// <param name="userId"></param>
		public void RemoveUser(long userId)
		{
			UserHub.UserConnections.TryRemove(userId, out var removedModel);
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
			var model = UserHub.GetConnectionModel(userId);
			return model != null ? model : null;
		}

		public async Task ReceiveMessage(long userId, string progress)
		{
			var connectionId = UserHub.GetConnectionId(userId);
			if (!string.IsNullOrEmpty(connectionId))
			{
				await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", progress);
			}
		}

		public async Task SendError(long userId, string error)
		{
			var connectionId = UserHub.GetConnectionId(userId);
			if (!string.IsNullOrEmpty(connectionId))
			{
				await _hubContext.Clients.Client(connectionId).SendAsync("ExportError", error);
			}
		}

		public async Task NotifyCompletion(long userId, string message)
		{
			var connectionId = UserHub.GetConnectionId(userId);
			if (!string.IsNullOrEmpty(connectionId))
			{
				await _hubContext.Clients.Client(connectionId).SendAsync("ExportCompleted", message);
			}
		}

		public async Task SendProgress(long userId, string message)
		{
			var connectionId = UserHub.GetConnectionId(userId);
			if (!string.IsNullOrEmpty(connectionId))
			{
				await _hubContext.Clients.Client(connectionId).SendAsync("SendProgress", message);
			}
		}

		public async Task SendNotificationToUser(long userId, string message)
		{
			var connectionId = UserHub.GetConnectionId(userId);
			if (!string.IsNullOrEmpty(connectionId))
			{
				await _hubContext.Clients.Client(connectionId).SendAsync("SendNotificationToUser", message);
			}
		}

		public async Task NoticeAllUserToGetOnlineStatus()
		{
			await _hubContext.Clients.All.SendAsync("NoticeAllUserToGetOnlineStatus", "拉取在线时间");
		}

		public async Task SendNotificationToRole(string role, string message)
		{
			await _hubContext.Clients.Group(role).SendAsync("SendNotificationToRole", message);
		}
	}

}
