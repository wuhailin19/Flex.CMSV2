using Flex.Application.Contracts.ISignalRBus.IServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.SignalRBus.Services
{
    public class NoticeOnlineStatusService : BackgroundService
    {
        private readonly IHubNotificationService _hubContext;
        private readonly ILogger<NoticeOnlineStatusService> _logger;

        public NoticeOnlineStatusService(IHubNotificationService hubContext, ILogger<NoticeOnlineStatusService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 向所有连接的客户端发送消息
                    //await _hubContext.Clients.All.SendAsync("ReceiveMessage", "CheckOnlineTime", stoppingToken);

                    // 等待10分钟
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // 任务取消时不处理异常
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in NoticeOnlineStatusService.");
                }
            }
        }
    }
}
