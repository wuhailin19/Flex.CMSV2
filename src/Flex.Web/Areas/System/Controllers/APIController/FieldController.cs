using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldController : ApiBaseController
    {
        private IFieldServices _services;
        public FieldController(IFieldServices services) {
            _services = services;
        }
        [HttpGet("Column")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<FieldColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync/{Id}")]
        public async Task<string> ListAsync(int Id) {
         return Success(await _services.ListAsync(Id));
        }

        //[HttpGet("GetFormHtml")]
        //public string GetFormHtml() {
        //    return Success(_services.GetFormHtml("用户名","Account", "required"));
        //}

        [HttpGet("GetFiledInfoById/{Id}")]
        public async Task<string> GetFiledInfoById(string Id) {
            return Success(await _services.GetFiledInfoById(Id));
        }

        [HttpPut]
        public async Task<string> Add() {
            var validate = await ValidateModel<AddFieldDto>();
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
            var validate = await ValidateModel<UpdateFieldDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.Update(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
    }
}
