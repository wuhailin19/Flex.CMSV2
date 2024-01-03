using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColumnContentController : ApiBaseController
    {
        private IColumnServices _columnServices;
        public ColumnContentController(IColumnServices columnServices) {
            _columnServices = columnServices;
        }
        [HttpGet("Column")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<ColumnContentDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        public async Task<string> ListAsync() {
         return Success(await _columnServices.ListAsync());
        }

        [HttpPut]
        public async Task<string> AddColumn() {
            var validate = await ValidateModel<AddColumnDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result =await  _columnServices.AddColumn(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        
        [HttpPost]
        public async Task<string> UpdateColumn() {
            var validate = await ValidateModel<UpdateColumnDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result =await  _columnServices.UpdateColumn(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
    }
}
