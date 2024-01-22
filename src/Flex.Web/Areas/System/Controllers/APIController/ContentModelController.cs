using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ContentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentModelController : ApiBaseController
    {
        private IContentModelServices _services;
        public ContentModelController(IContentModelServices services) {
            _services = services;
        }
        [HttpGet("Column")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<ContentModelColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        public async Task<string> ListAsync() {
         return Success(await _services.ListAsync());
        }

        [HttpGet("GetSelectItem")]
        public async Task<string> GetSelectItem() {
            return Success(await _services.GetSelectItem());
        }

        [HttpPut]
        public async Task<string> Add() {
            var validate = await ValidateModel<AddContentModelDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result =await _services.Add(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpDelete("{Id}")]
        public async Task<string> Delete(string Id)
        {
            var result = await _services.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost]
        public async Task<string> Update()
        {
            var validate = await ValidateModel<UpdateContentModelDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.Update(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        
        [HttpPost("UpdateFormString")]
        public async Task<string> UpdateFormString()
        {
            var validate = await ValidateModel<UpdateFormHtmlStringDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.UpdateFormString(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
    }
}
