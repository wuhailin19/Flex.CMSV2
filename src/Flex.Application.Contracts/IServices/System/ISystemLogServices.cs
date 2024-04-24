using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Enums.LogLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices.System
{
    [NoLog]
    public interface ISystemLogServices
    {
        Task AddContentLog(SystemLogLevel systemLogLevel, string operationContent, string request);
        Task AddLog(InputSystemLogDto log);
        Task AddLoginLog(LoginSystemLogDto loginSystemLogDto);
        Task<PagedList<SystemLogColumnDto>> ListAsync(int page, int limit, LogSort LogSort, SystemLogLevel LogLevel = SystemLogLevel.All, string msg = null);
    }
}
