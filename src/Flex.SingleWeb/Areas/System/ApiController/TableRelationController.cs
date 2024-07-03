using Flex.Application.Contracts.IServices.System;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.Normal.ProductManage;
using Flex.Domain.Dtos.System.TableRelation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ApiController
{
    [Route("api/[controller]")]
    public class TableRelationController : ApiBaseController
    {
        ITableRelationServices _services;
        public TableRelationController(ITableRelationServices tableRelationServices) {
            _services = tableRelationServices;
        }

        [HttpGet("Column")]
        [AllowAnonymous]
        [Descriper(IsFilter = true)]
        public string Column()
        {
            return Success(ModelTools<TableRelationColumnDto>.getColumnDescList());
        }

        [HttpGet("ListAsync")]
        [Descriper(Name = "获取账号列表")]
        public async Task<string> ListAsync(int page, int limit)
        {
            return Success(await _services.ListAsync(page, limit));
        }

        [HttpPost("Add")]
        public async Task<string> AddTableRelation()
        {
            var validate = await ValidateModel<AddTableRelationDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.AddTableRelation(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("Update")]
        public async Task<string> UpdateTableRelation()
        {
            var validate = await ValidateModel<UpdateTableRelationDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.UpdateTableRelation(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("{Id}")]
        [Descriper(Name = "删除角色")]
        public async Task<string> Delete(string Id)
        {
            var result = await _services.DeleteTableRelation(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
