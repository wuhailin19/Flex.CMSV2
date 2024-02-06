using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Flex.Dapper;
using System.Collections;

namespace Flex.Web.Areas.System.Controllers.APIController
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
        [HttpGet("Column/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> Column(int ParentId)
        {
            return Success(await _columnServices.GetTableThs(ParentId));
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
        public async Task<string> ListAsync(int page, int limit, int ParentId)
        {
            return Success(await _columnServices.ListAsync(page, limit, ParentId));
        }

        [HttpGet("GetFormHtml/{ParentId}")]
        [Descriper(Name = "通过ParentId获取模型结构代码")]
        public async Task<string> GetFormHtml(int ParentId)
        {
            var result = await _columnServices.GetFormHtml(ParentId);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpGet("GetContentById/{ParentId}/{Id}")]
        [Descriper(Name = "通过ParentId和Id获取栏目内容",IsFilter =true)]
        [AllowAnonymous]
        public async Task<string> GetContentById(int ParentId, int Id)
        {
            return Success(await _columnServices.GetContentById(ParentId, Id));
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

        [HttpPost("{ParentId}/{Id}")]
        [Descriper(Name = "删除栏目内容")]
        public async Task<string> Delete(int ParentId,string Id)
        {
            var result = await _columnServices.Delete(ParentId,Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
