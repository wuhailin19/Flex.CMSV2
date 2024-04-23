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
    public interface ISystemLogServices
    {
        [NoLog]
        Task AddContentLog(SystemLogLevel systemLogLevel, string operationContent, string request);
        [NoLog]
        Task AddLog(InputSystemLogDto log);
        [NoLog]
        Task AddLoginLog(LoginSystemLogDto loginSystemLogDto);
        [NoLog]
        Task<PagedList<SystemLogColumnDto>> ListAsync(int page, int limit, LogSort LogSort, SystemLogLevel LogLevel = SystemLogLevel.All, string msg = null);
    }
}
