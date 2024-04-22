using Flex.Application.Contracts.Basics.ResultModels;
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
        Task<ProblemDetails<string>> AddContentLog(SystemLogLevel systemLogLevel, string operationContent, string request);
        Task<ProblemDetails<string>> AddLog(InputSystemLogDto log);
        Task<ProblemDetails<string>> AddLoginLog(LoginSystemLogDto loginSystemLogDto);
        Task<PagedList<SystemLogColumnDto>> ListAsync(int page, int limit, LogSort LogSort, SystemLogLevel LogLevel = SystemLogLevel.Normal, string msg = null);
    }
}
