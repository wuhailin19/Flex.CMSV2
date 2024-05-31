using Castle.Core.Internal;
using Flex.Application.ContentModel;
using Flex.Core.Attributes;
using Flex.Core.Config;
using Flex.Core.Helper;
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
            string swhere = "IsHide=0 and ParentId in(" + columnId + ")";
            var contentDto = ContentModelHelper.GetProductTableColumnHashTable(columnId);
            int recount = 0;

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
            string jsonstr = string.Empty;

            string urlext = ServerConfig.ServerUrl
                 + "JsonDocx/GetPageContentByColumnId?page="
                 + inputJsondocxDto.page + "&pagesize="
                 + inputJsondocxDto.pagesize + "&columnId="
                 + inputJsondocxDto.columnId;

            if (inputJsondocxDto.modelId != 0)
                urlext = ServerConfig.ServerUrl +
                    "JsonDocx/GetPageContentByModelId?page="
                    + inputJsondocxDto.page + "&pagesize="
                    + inputJsondocxDto.pagesize + "&columnId="
                    + inputJsondocxDto.columnId + "&modelId="
                    + inputJsondocxDto.modelId;

            if (!inputJsondocxDto.k.IsEmpty())
                urlext += "&k=" + inputJsondocxDto.k;

            Dictionary<object, List<FiledModel>> full_fileds = new Dictionary<object, List<FiledModel>>();
            ColumnJsonDocxDto columnJsonDocxDto = new ColumnJsonDocxDto();
            #region 拼接条件
            //columnId = "61,62,63";
            int recount = 0;
            string swhere = "IsHide=0";
            if (inputJsondocxDto.columnId.IsNotNullOrEmpty())
                swhere += " and ParentId in(" + inputJsondocxDto.columnId + ")";
            if (inputJsondocxDto.PId != 0 && inputJsondocxDto.modelId != 0)
                swhere += " and PId=" + inputJsondocxDto.PId;
            var contentDto = new List<ContentModelDto>();
            if (inputJsondocxDto.modelId != 0)
            {
                contentDto = new List<ContentModelDto>() { ContentModelHelper.GetModelContentByModelId(inputJsondocxDto.modelId) };
            }
            else
            {
                contentDto = ContentModelHelper.GetProductTableColumnHashTable(inputJsondocxDto.columnId);
            }

            #endregion
            if (contentDto.Count == 0)
            {
                jsonstr += "没有内容";
            }
            else
            {
                if (contentDto.Count == 1)
                {
                    #region 单个ID
                    var fileds = contentDto[0].TableColumnList;
                    if (!inputJsondocxDto.k.IsNullOrEmpty())
                    {
                        var searchlist = fileds.Where(m => m.IsSearch).ToList();
                        swhere += ContentModelHelper.GetExpressionByFiledNameList(searchlist, inputJsondocxDto.k);
                    }
                    var model = ContentModelHelper.GetJsonDataByContentDtoPageDocx(contentDto[0], inputJsondocxDto.page, inputJsondocxDto.pagesize, swhere, ref recount);
                    ContentModelPage contentModelPage = new ContentModelPage()
                    {
                        page = inputJsondocxDto.page,
                        dataCount = recount,
                        pagesize = inputJsondocxDto.pagesize,
                        data = model
                    };
                    string tbname = ContentModelHelper.GetvirtualTableName(contentDto[0].TableName);
                    full_fileds.Add(tbname + "：【" + contentDto[0].ModelName + "】", fileds);
                    Dictionary<string, string> filedlist = new Dictionary<string, string>
                    {
                        { "code","code"},
                        {"data","data" },
                        {"page","page" },
                        {"pagesize","pagesize" },
                        {"dataCount","dataCount" },
                        {"pageCount","pageCount" },
                        {"msg","msg" },
                        { tbname ,tbname}
                    };
                    foreach (var item in fileds)
                    {
                        if (filedlist.ContainsKey(tbname + item.FiledName))
                            continue;
                        filedlist.Add(tbname + item.FiledName, item.FiledName);
                    }
                    jsonstr = JsonHelper.ToJson(contentModelPage);
                    //var regexmodel= Regex.Matches(jsonstr, "([^>]\"[a-zA-Z]+\":)");
                    foreach (var item in filedlist.Keys)
                    {
                        jsonstr = jsonstr.Replace("\"" + item + "\"", "<span class='json-property' href='#" + item + "'>\"" + filedlist[item] + "\"</span>");
                    }
                    #endregion
                }
                else
                {
                    #region 多个ID
                    Dictionary<string, object> contents = new Dictionary<string, object>();
                    Dictionary<string, string> filedlist = new Dictionary<string, string>
                    {
                        {"code","code"},
                        { "data", "data" },
                        { "msg", "msg" },
                        { "PageModel", "PageModel" }
                    };
                    string oldswhere = swhere;
                    foreach (var firstDto in contentDto)
                    {
                        swhere = oldswhere;
                        if (!inputJsondocxDto.k.IsNullOrEmpty())
                        {
                            var searchlist = firstDto.TableColumnList.Where(m => m.IsSearch).ToList();
                            swhere += ContentModelHelper.GetExpressionByFiledNameList(searchlist, inputJsondocxDto.k);
                        }
                        var model = ContentModelHelper.GetJsonDataByContentDtoPageDocx(firstDto, swhere);
                        string tbname = ContentModelHelper.GetvirtualTableName(firstDto.TableName);
                        if (contents.ContainsKey(tbname))
                            continue;
                        contents.Add(tbname, model);

                        var nowfileds = firstDto.TableColumnList;
                        full_fileds.Add(tbname + "：【" + firstDto.ModelName + "】", nowfileds);
                        foreach (var item in nowfileds)
                        {
                            if (filedlist.ContainsKey(tbname + item.FiledName))
                                continue;
                            filedlist.Add(tbname + item.FiledName, item.FiledName);
                        }
                        if (filedlist.ContainsKey(tbname))
                            continue;
                        else
                            filedlist.Add(tbname, tbname);
                    }

                    jsonstr = JsonHelper.ToJson(contents);
                    foreach (var item in filedlist.Keys)
                    {
                        jsonstr = jsonstr.Replace("\"" + item + "\"", "<span class='json-property' href='#" + item + "'>\"" + filedlist[item] + " \"</span>");
                    }
                    #endregion
                }
            }
            columnJsonDocxDto.full_fileds = full_fileds;
            columnJsonDocxDto.JsonStr = jsonstr;
            columnJsonDocxDto.urlext = urlext;
            return Success(columnJsonDocxDto);
        }
    }
}
