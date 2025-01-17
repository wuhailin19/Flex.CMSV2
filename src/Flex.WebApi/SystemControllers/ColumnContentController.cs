﻿using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Flex.Dapper;
using System.Collections;
using Flex.Domain.Dtos.ColumnContent;

namespace Flex.WebApi.SystemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "栏目内容相关接口")]
    public class ColumnContentController : ApiBaseController
    {
        private IColumnContentServices _columnServices;
        public ColumnContentController(IColumnContentServices columnServices)
        {
            _columnServices = columnServices;
        }
        [HttpGet("Column/{modelId}/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> Column(int modelId, int ParentId)
        {
            return Success(await _columnServices.GetTableThs(ParentId, modelId));
        }

        [HttpGet("HistoryColumn/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> HistoryColumn(int ParentId)
        {
            return Success(await _columnServices.GetHistoryTableThs(ParentId));
        }

        [HttpGet("SoftDeleteColumn/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> SoftDeleteColumn(int ParentId)
        {
            return Success(await _columnServices.GetSoftTableThs(ParentId));
        }

        /// <summary>
        /// 多选checkbox数据用此接口
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        [HttpGet("ContentOptions/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> GetContentOptions(int ParentId)
        {
            return Success(await _columnServices.GetContentOptions(ParentId));
        }

        [HttpGet("ListAsync")]
        [Descriper(Name = "栏目内容列表分页数据")]
        public async Task<string> ListAsync([FromQuery] ContentPageListParamDto model)
        {
            if (model == null)
                return Fail("无数据");
            return Success(await _columnServices.ListAsync(model));
        }

        [HttpGet("HistoryListAsync")]
        [Descriper(Name = "历史内容列表分页数据")]
        public async Task<string> HistoryListAsync([FromQuery] ContentPageListParamDto model)
        {
            if (model == null)
                return Fail("无数据");
            return Success(await _columnServices.HistoryListAsync(model));
        }

        [HttpGet("SoftDeleteListAsync")]
        [Descriper(Name = "回收站列表分页数据")]
        public async Task<string> SoftDeleteListAsync([FromQuery] ContentPageListParamDto model)
        {
            if (model == null)
                return Fail("无数据");
            return Success(await _columnServices.SoftDeleteListAsync(model));
        }

        [HttpGet("GetFormHtml/{currentmodelId}")]
        [Descriper(Name = "通过ParentId获取模型结构代码")]
        public async Task<string> GetFormHtml(int currentmodelId)
        {
            var result = await _columnServices.GetFormHtml(currentmodelId);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpGet("GetContentById/{modelId}/{ParentId}/{Id}")]
        [Descriper(Name = "通过modelId、ParentId和Id获取栏目内容", IsFilter = true)]
        public async Task<string> GetContentById(int modelId, int ParentId, int Id)
        {
            if (Id == 0)
                return Success(await _columnServices.GetButtonListByParentId(ParentId));
            var result = await _columnServices.GetContentById(modelId, ParentId, Id);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Content);
        }

        [HttpPost("CreateColumnContent")]
        [Descriper(Name = "新增栏目内容")]
        public async Task<string> Add()
        {
            var model = await GetModel<Hashtable>();
            var result = await _columnServices.Add(model);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost]
        [Descriper(Name = "修改栏目内容")]
        public async Task<string> Update()
        {
            var model = await GetModel<Hashtable>();
            var result = await _columnServices.Update(model);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("UpdateStatus")]
        [Descriper(Name = "修改栏目内容状态")]
        public async Task<string> UpdateStatus()
        {
            var model = await GetModel<Hashtable>();
            var result = await _columnServices.SimpleUpdateContent(model);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("CancelReview")]
        [Descriper(Name = "取消审批")]
        public async Task<string> CancelReview()
        {
            var model = await GetModel<Hashtable>();
            model["ReviewAddUser"] = string.Empty;
            model["MsgGroupId"] = string.Empty;
            var result = await _columnServices.UpdateReviewContent(model, true, true);
            if (!result.IsSuccess)
                return Fail("审批取消失败");
            return Success(result.Detail);
        }

        [HttpPost("CompletelyDelete/{modelId}/{parentId}/{Id}")]
        [Descriper(Name = "完全删除栏目内容，不可恢复")]
        public async Task<string> CompletelyDelete(int modelId, int parentId, string Id)
        {
            var result = await _columnServices.CompletelyDelete(parentId, modelId, Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("RestContent/{modelId}/{parentId}/{Id}")]
        [Descriper(Name = "从回收站恢复栏目内容")]
        public async Task<string> RestContent(int modelId, int parentId, string Id)
        {
            var result = await _columnServices.RestContent(parentId, modelId, Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("{modelId}/{parentId}/{Id}")]
        [Descriper(Name = "软删除栏目内容，可恢复")]
        public async Task<string> Delete(int modelId, int parentId, string Id)
        {
            var result = await _columnServices.Delete(parentId, modelId, Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
