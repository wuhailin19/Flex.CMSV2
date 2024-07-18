using Flex.Application.Authorize;
using Flex.Application.Contracts.ISignalRBus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.ISignalRBus.IServices
{
    public interface IHubNotificationService
    {
        Task SendProgress(long userId, string progress);
        Task SendError(long userId, string error);
        Task NotifyCompletion(long userId, string message);
        Task SendNotificationToUser(long userId, string message);
        Task SendNotificationToRole(string role, string message);
        ConnectionModel GetClaims(long userId);
    }
}
