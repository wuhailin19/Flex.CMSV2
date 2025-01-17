﻿using Flex.Core.Attributes;
using Flex.Domain.Dtos.WorkFlow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.WebApi.SystemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "工作流相关接口")]
    public class WorkFlowController : ApiBaseController
    {
        IWorkFlowServices _workFlowServices;
        public WorkFlowController(IWorkFlowServices workFlowServices)
        {
            _workFlowServices = workFlowServices;
        }

        [HttpGet("Column")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<WorkFlowColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        [Descriper(Name = "获取工作流列表")]
        public async Task<string> ListAsync(int page, int limit)
        {
            return Success(await _workFlowServices.GetWorkFlowListAsync(page, limit));
        }

        [HttpGet("GetWorkFlowSelectDtoListAsync")]
        [Descriper(Name = "获取工作流下拉集合")]
        public async Task<string> GetWorkFlowSelectDtoListAsync()
        {
            return Success(await _workFlowServices.GetWorkFlowSelectDtoListAsync());
        }

        [HttpPost("GetStepActionButtonList")]
        [Descriper(Name = "获取当前动作的审核按钮列表")]
        public async Task<string> GetStepActionButtonList()
        {
            var model = await GetModel<InputWorkFlowStepDto>();
            return Success(await _workFlowServices.GetStepActionButtonList(model));
        }
        /// <summary>
        /// 添加工作流
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateWorkFlow")]
        [Descriper(Name = "添加工作流")]
        public async Task<string> Add()
        {
            var validate = await ValidateModel<InputWorkFlowAddDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _workFlowServices.Add(validate.Content);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        /// <summary>
        /// 修改流程图
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateInfo")]
        [Descriper(Name = "编辑工作流")]
        public async Task<string> UpdateInfo()
        {
            var model = await GetModel<InputWorkFlowUpdateDto>();
            var result = await _workFlowServices.Update(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        /// <summary>
        /// 修改流程图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Descriper(Name = "修改流程图")]
        public async Task<string> UpdateFlowChat()
        {
            var model = await GetModel<InputWorkFlowContentDto>();
            var result = await _workFlowServices.UpdateFlowChat(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpGet("GetStepManagerById")]
        [Descriper(IsFilter = true)]
        public async Task<string> GetStepManagerById(string Id)
        {
            var result = await _workFlowServices.GetStepManagerById(Id);
            return Success(result);
        }
        /// <summary>
        /// 修改流程图
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateStepManager")]
        [Descriper(Name = "编辑工作流步骤参与人")]
        public async Task<string> UpdateStepManager()
        {
            var model = await GetModel<InputEditStepManagerDto>();
            var result = await _workFlowServices.UpdateStepManager(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("{Id}")]
        [Descriper(Name = "删除工作流")]
        public async Task<string> Delete(string Id)
        {
            var result = await _workFlowServices.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

    }
}
