using Flex.Core.Attributes;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.System.ContentModel;
using Flex.Domain.Dtos.System.TableRelation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.WebApi.SystemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "内容模型相关接口")]
    public class ContentModelController : ApiBaseController
    {
        private IContentModelServices _services;
        public ContentModelController(IContentModelServices services)
        {
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
        [Descriper(Name = "内容模型列表数据")]
        public async Task<string> ListAsync()
        {
            return Success(await _services.ListAsync());
        }

        [HttpGet("GetSelectItem")]
        [Descriper(Name = "栏目管理内容模型选择项数据")]
        public async Task<string> GetSelectItem()
        {
            return Success(await _services.GetSelectItem());
        }

        [HttpGet("GetFormHtml/{modelId}")]
        [Descriper(Name = "通过模型Id获取模型结构代码")]
        public async Task<string> GetFormHtml(int modelId)
        {
            var result = await _services.GetFormHtml(modelId);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("CreateContentModel")]
        [Descriper(Name = "新增模型")]
        public async Task<string> Add()
        {
            var validate = await ValidateModel<AddContentModelDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.Add(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("{Id}")]
        [Descriper(Name = "删除模型")]
        public async Task<string> Delete(string Id)
        {
            var result = await _services.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost]
        [Descriper(Name = "修改模型名和描述")]
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

        [HttpPost("QuickEdit")]
        [Descriper(Name = "快速修改模型状态")]
        public async Task<string> QuickEdit()
        {
            var validate = await ValidateModel<QuickEditContentModelDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.QuickEdit(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("UpdateFormString")]
        [Descriper(Name = "修改模型结构代码")]
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
