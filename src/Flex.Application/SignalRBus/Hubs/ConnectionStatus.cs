using Flex.Core.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.SignalRBus.Hubs
{
	public class ConnectionStatus
	{
		private readonly ConcurrentDictionary<string, ConnectionInfo> _connections = new();
		private readonly double _defaultExpiration = ServerConfig.SignalRExpiryTime;
		private readonly TimeSpan _refreshThreshold = TimeSpan.FromMinutes(5);

		public void AddOrUpdateConnection(string token, string connectionId)
		{
			// 检查现有 token 的信息
			if (_connections.TryGetValue(token, out var existingInfo))
			{
				// 如果现有信息存在，检查过期时间
				if (existingInfo.Expiration - Clock.Now <= _refreshThreshold)
				{
					// 如果离过期时间不足 5 分钟，延长过期时间
					existingInfo.Expiration = Clock.Now.AddMinutes(_defaultExpiration);
				}

				// 更新连接 ID（即使没有延长过期时间，也需要更新连接 ID）
				existingInfo.ConnectionId = connectionId;
			}
			else
			{
				// 如果没有现有信息，创建新的连接信息
				var expiration = Clock.Now.AddMinutes(_defaultExpiration);
				_connections[token] = new ConnectionInfo(connectionId, expiration);
			}
		}

		public string GetConnectionId(string token, out DateTime expiration)
		{
			if (_connections.TryGetValue(token, out var connectionInfo))
			{
				expiration = connectionInfo.Expiration;

				// 如果离过期时间还有5分钟或更少，延长过期时间
				if (expiration - Clock.Now <= _refreshThreshold)
				{
					connectionInfo.Expiration = Clock.Now.AddMinutes(_defaultExpiration);
					_connections[token] = connectionInfo;
				}

				return connectionInfo.ConnectionId;
			}
			expiration = default;
			return null;
		}

		public void RemoveConnection(string token)
		{
			_connections.TryRemove(token, out _);
		}

		private class ConnectionInfo
		{
			public string ConnectionId { get; set; }
			public DateTime Expiration { get; set; }

			public ConnectionInfo(string connectionId, DateTime expiration)
			{
				ConnectionId = connectionId;
				Expiration = expiration;
			}
		}
	}
}
