using Flex.Core.Attributes;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using Flex.Domain.Dtos.System.ColumnContent;
using Flex.Domain.Dtos.System.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Flex.WebApi.SystemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "模型字段相关接口")]
    public class FieldController : ApiBaseController
    {
        private IFieldServices _services;
        public FieldController(IFieldServices services)
        {
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
        [Descriper(Name = "模型字段列表数据")]
        public async Task<string> ListAsync(int Id)
        {
            return Success(await _services.ListAsync(Id));
        }

        [HttpGet("GetFieldDictAsync/{Id}")]
        [Descriper(Name = "模型字段字典数据")]
        public async Task<string> GetFieldDictAsync(int Id)
        {
            var data = await _services.ListAsync(Id);

            ContentImpoertExcelDto contentImpoertExcelDto = new ContentImpoertExcelDto();
            contentImpoertExcelDto.Fileds.Add("Title", "标题");
            contentImpoertExcelDto.Fileds.Add("AddTime", "添加时间");
            contentImpoertExcelDto.Fileds.Add("PublishTime", "发布时间");
            if (data != null && data.Count() > 0)
            {
                foreach (var item in data)
                {
                    contentImpoertExcelDto.Fileds.Add(item.FieldName, item.Name);
                }
            }
            return Success(contentImpoertExcelDto);
        }

        //[HttpGet("GetFormHtml")]
        //public string GetFormHtml() {
        //    return Success(_services.GetFormHtml("用户名","Account", "required"));
        //}

        [HttpGet("GetFiledInfoById/{Id}")]
        [Descriper(Name = "通过Id获取字段数据")]
        public async Task<string> GetFiledInfoById(string Id)
        {
            return Success(await _services.GetFiledInfoById(Id));
        }

        [Descriper(IsFilter = true)]
        private async Task<string> Add()
        {
            var validate = await ValidateModel<AddFieldDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.Add(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpDelete("{Id}")]
        [Descriper(IsFilter = true)]
        private async Task<string> Delete(string Id)
        {
            var result = await _services.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost]
        [Descriper(Name = "通过Id修改字段数据")]
        private async Task<string> Update()
        {
            var validate = await ValidateModel<UpdateFieldDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.Update(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("UpdateApiName")]
        [Descriper(Name = "通过Id修改字段接口别名")]
        public async Task<string> UpdateApiName()
        {
            var validate = await ValidateModel<SimpleUpdateDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.SimpleUpdateDto(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("QuickEdit")]
        [Descriper(Name = "快速修改字段状态")]
        public async Task<string> QuickEdit()
        {
            FiledQuickEditDto fieldQuickEdit = await GetModel<FiledQuickEditDto>();
            var result = await _services.QuickEditField(fieldQuickEdit);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
