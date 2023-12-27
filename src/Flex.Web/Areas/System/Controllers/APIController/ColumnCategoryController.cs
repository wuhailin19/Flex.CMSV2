using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColumnCategoryController : ApiBaseController
    {
        private IColumnServices _columnServices;
        public ColumnCategoryController(IColumnServices columnServices) {
            _columnServices = columnServices;
        }
        [HttpGet("Column")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<ColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        public async Task<string> ListAsync() {
         return Success(await _columnServices.GetTreeColumnListDtos());
        }

    }
}
