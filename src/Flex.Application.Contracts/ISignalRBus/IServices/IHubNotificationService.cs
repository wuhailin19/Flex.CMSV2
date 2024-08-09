using Flex.Application.Authorize;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Domain.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.ISignalRBus.IServices
{
    public interface IHubNotificationService
    {
        Task SendProgress(long userId, string message);
        Task SendError(long userId, string error);
        Task NotifyCompletion(long userId, string message);
        Task SendNotificationToUser(long userId, string message);
        Task SendNotificationToRole(string role, string message);
        ConnectionModel GetClaims(long userId);
        Task ReceiveMessage(long userId, string progress);
        ConnectionModel GetConnectionModel(long userId);
	}
}
