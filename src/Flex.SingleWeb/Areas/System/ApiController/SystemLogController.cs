using Flex.Application.Contracts.IServices.System;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Enums.LogLevel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ApiController
{
    [Route("api/[controller]")]
    [Descriper(Name = "日志相关接口")]
    public class SystemLogController : ApiBaseController
    {
        ISystemLogServices _systemLogServices;
        public SystemLogController(ISystemLogServices systemLogServices)
        {
            _systemLogServices = systemLogServices;
        }
        [HttpGet("Column")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<SystemLogColumnDto>.getColumnDescList());
        }

        [HttpGet("ListAsync")]
        [Descriper(Name = "日志列表")]
        public async Task<string> ListAsync(int page, int limit, LogSort logSort, SystemLogLevel logLevel= SystemLogLevel.All, string msg = null)
        {
            return Success(await _systemLogServices.ListAsync(page, limit, logSort, logLevel, msg));
        }
    }
}
