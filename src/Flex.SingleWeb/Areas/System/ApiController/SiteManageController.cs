using Autofac.Core;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using Flex.Domain.Dtos.Menu;
using Flex.Domain.Dtos.System.SiteManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    [Descriper(Name = "站点管理相关接口")]
    public class SiteManageController : ApiBaseController
    {
        private ISiteManageServices _Services;
        public SiteManageController(ISiteManageServices Services)
        {
            _Services = Services;
        }
        [HttpGet("Column")]
        [AllowAnonymous]
        [Descriper(IsFilter = true)]
        public string Column()
        {
            return Success(ModelTools<SiteManageColumnDto>.getColumnDescList());
        }

        [HttpGet("ListAsync")]
        [Descriper(Name = "站点管理页面数据")]
        public async Task<string> ListAsync()
        {
            return Success((await _Services.ListAsync()));
        }

        [Descriper(Name = "新增站点")]
        [HttpPost("CreateSiteManage")]
        public async Task<string> Add()
        {
            var validate = await ValidateModel<AddSiteManageDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _Services.AddSite(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost]
        [Descriper(Name = "修改站点")]
        public async Task<string> Update()
        {
            var validate = await ValidateModel<UpdateSiteManageDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _Services.UpdateSite(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("{Id}")]
        [Descriper(Name = "删除站点")]
        public async Task<string> Delete(string Id)
        {
            var result = await _Services.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
