using Castle.Core.Internal;
using Flex.Application.ContentModel;
using Flex.Core.Attributes;
using Flex.Core.Config;
using Flex.Core.Helper;
using Flex.Core.Timing;
using Flex.Domain.Dtos.System.Column;
using Flex.Domain.Dtos.System.ContentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShardingCore.Extensions;
using SqlSugar;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Drawing.Printing;

namespace Flex.SingleWeb.Areas.System.ApiController
{
    [Route("api/[controller]")]
    [Descriper(Name = "栏目数据相关接口", IsFilter = true)]
    public class JsonDocxController : ApiBaseController
    {
        [HttpGet("GetContentByColumnId")]
        [AllowAnonymous]
        public string GetContentByColumnId(string columnId)
        {
            var contentDto = ContentModelHelper.GetProductTableColumnHashTable(columnId);
            return Success(contentDto);
        }

        [HttpGet("GetPageContentByColumnId")]
        [AllowAnonymous]
        public string GetPageContentByColumnId(string columnId, int page, int pagesize, string k)
        {
            string swhere = $"IsHide=0 and StatusCode=1 and PublishTime<='{Clock.Now}' and ParentId in({columnId})";
            var contentDto = ContentModelHelper.GetProductTableColumnHashTable(columnId);
            int recount = 0;
            if (contentDto == null)
                return Fail("数据为空");

            if (contentDto.Count == 0)
                return Fail("数据为空");

            if (contentDto.Count == 1)
            {
                if (!k.IsEmpty())
                {
                    var searchlist = contentDto[0].TableColumnList.Where(m => m.IsSearch).ToList();
                    swhere += ContentModelHelper.GetExpressionByFiledNameList(searchlist, k);
                }
                var model = ContentModelHelper.GetJsonDataByContentDto(contentDto[0], page, pagesize, swhere, ref recount);
                ContentModelPage contentModelPage = new ContentModelPage()
                {
                    page = page,
                    dataCount = recount,
                    pagesize = pagesize,
                    data = model
                };
                return Success(contentModelPage);
            }
            else
            {
                Dictionary<string, object> contents = new Dictionary<string, object>();
                var oldswhere = swhere;
                foreach (var firstDto in contentDto)
                {
                    swhere = oldswhere;
                    if (!k.IsEmpty())
                    {
                        var searchlist = firstDto.TableColumnList.Where(m => m.IsSearch).ToList();
                        swhere += ContentModelHelper.GetExpressionByFiledNameList(searchlist, k);
                    }
                    var model = ContentModelHelper.GetJsonDataByContentDto(firstDto, swhere);

                    contents.Add(ContentModelHelper.GetvirtualTableName(firstDto.TableName), model);
                }
                return Success(contents);
            }
        }

        [HttpGet("GetPageContentByModelId")]
        [AllowAnonymous]
        public string GetPageContentByModelId(int modelId, int page, int pagesize, string columnId, string PId, string k)
        {
            string swhere = "IsHide=0";
            if (columnId.IsNotNullOrEmpty())
                swhere += " and ParentId in(" + columnId + ")";
            if (!PId.IsEmpty())
                swhere += " and PId=" + PId;
            var contentDto = ContentModelHelper.GetModelContentByModelId(modelId);
            if (!k.IsEmpty())
            {
                var searchlist = contentDto.TableColumnList.Where(m => m.IsSearch).ToList();
                swhere += ContentModelHelper.GetExpressionByFiledNameList(searchlist, k);
            }
            int recount = 0;
            var list = ContentModelHelper.GetJsonDataByContentDto(contentDto, page, pagesize, swhere, ref recount);
            ContentModelPage contentModelPage = new ContentModelPage()
            {
                page = page,
                dataCount = recount,
                pagesize = pagesize,
                data = list
            };
            return Success(contentModelPage);
        }

        [HttpGet("GetJsonDocxContent")]
        [AllowAnonymous]
        public string GetJsonDocxContent(InputJsondocxDto inputJsondocxDto)
        {
            ColumnJsonDocxDto columnJsonDocxDto = ContentModelHelper.GetContentJsonByExpression(inputJsondocxDto);
            return Success(columnJsonDocxDto);
        }

        [HttpGet("ExportExcel")]
        [AllowAnonymous]
        public IActionResult ExportExcel()
        {
            var model = ContentModelHelper.GetModelContentByModelId(4);
            var stream = ContentModelHelper.SimpleExportToSpreadsheet("测试", model.TableColumnList);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "测试.xlsx";
            return File(stream, contentType, fileName);
        }
    }
}
