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
        
        //[HttpPost]
        //public async Task<string> UpdateColumn() {
        //    var validate = await ValidateModel<UpdateColumnDto>();
        //    if (!validate.IsSuccess)
        //        return Fail(validate.Detail);
        //    var result =await  _columnServices.UpdateColumn(validate.Content);
        //    if (!result.IsSuccess)
        //        return Fail(result.Detail);
        //    return Success(result.Detail);
        //}
    }
}
