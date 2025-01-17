﻿using Flex.Application.Authorize;
using Flex.Application.Contracts.ISignalRBus.Enum;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ApiController
{
    [Route("api/[controller]")]
    [Descriper(Name = "任务相关接口")]
    public class TaskController : ApiBaseController
    {
        private ITaskServices _taskServices;
        IClaimsAccessor _claims;
        public TaskController(
            ITaskServices taskServices,
            IClaimsAccessor claims
            )
        {
            _taskServices = taskServices;
            _claims = claims;
        }
        /// <summary>
        /// 获取正在等待的任务数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWaittingTaskCount")]
        [Descriper(Name = "获取正在等待的任务数量")]
        public string GetWaittingTaskCount()
        {
            var result = _taskServices.GetTaskModelsAsync(_claims.UserId);
            var count = result.Where(m => m.Status == GlobalTaskStatus.Waiting).Count();
            return Success(count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllTaskList")]
        [Descriper(Name = "获取自身所有任务")]
        public string GetAllTaskList()
        {
            var result = _taskServices.GetTaskModelsAsync(_claims.UserId);
            return Success(result.OrderByDescending(m => m.AddTime));
        }
    }
}
