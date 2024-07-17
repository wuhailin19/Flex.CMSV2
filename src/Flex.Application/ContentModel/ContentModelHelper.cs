using Flex.Core.Config;
using Flex.Domain.Dtos.System.Column;
using Flex.Domain.Dtos.System.ContentModel;
using Flex.SqlSugarFactory.Seed;
using Microsoft.AspNetCore.Http;
using ShardingCore.Extensions;
using SqlSugar;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaGetWxaGameFrameResponse.Types.Data.Types.Frame.Types;
using System.Collections.Generic;
using System.Collections;

namespace Flex.Application.ContentModel
{
    public class ContentModelHelper
    {
        public static DataTable _column = null;
        public static DataTable _contentlist = null;
        public static DataTable _filedlist = null;
        public static DataTable _relationlist = null;
        protected MyContext _sqlsugar;
        public static ContentModelHelper instance =
            Singleton<ContentModelHelper>.Instance
            ?? (Singleton<ContentModelHelper>.Instance = new ContentModelHelper());

        public ContentModelHelper()
        {
            _sqlsugar = new MyContext();
        }

        public static void clearData()
        {
            column = null;
            contentlist = null;
            filedlist = null;
            relationlist = null;


        }
        /// <summary>
        /// 栏目Id与内容模型对应表
        /// </summary>
        public static DataTable column
        {
            get
            {
                if (_column == null)
                {
                    Expression<Func<SysColumn, bool>> express = m => (int)m.StatusCode == 1;
                    var columnCategory = instance._sqlsugar.Db.Queryable<SysColumn>().Where(express).ToDataTable();
                    _column = columnCategory;
                }
                return _column;
            }
            set { _column = value; }
        }
        /// <summary>
        /// 内容表列表
        /// </summary>
        public static DataTable contentlist
        {
            get
            {
                if (_contentlist == null)
                {
                    Expression<Func<SysContentModel, bool>> express = m => (int)m.StatusCode == 1;
                    var ContentModel = instance._sqlsugar.Db.Queryable<SysContentModel>().Where(express).ToDataTable();
                    _contentlist = ContentModel;
                }
                return _contentlist;
            }
            set { _contentlist = value; }
        }
        /// <summary>
        /// 关系表列表
        /// </summary>
        public static DataTable relationlist
        {
            get
            {
                if (_relationlist == null)
                {
                    var ContentModel = instance._sqlsugar.Db.Ado.GetDataTable("select ParentModelId,ChildModelId from ChildRelation");
                    _relationlist = ContentModel;
                }
                return _relationlist;
            }
            set { _relationlist = value; }
        }
        /// <summary>
        /// 字段列表
        /// </summary>
        public static DataTable filedlist
        {
            get
            {
                if (_filedlist == null)
                {
                    Expression<Func<sysField, bool>> express = m => (int)m.StatusCode == 1;
                    var filedlist = instance._sqlsugar.Db.Queryable<sysField>().Where(express).ToDataTable();
                    _filedlist = filedlist;
                }
                return _filedlist;
            }
            set { _filedlist = value; }
        }
        #region 方法
        private static DataTable GetDataList(string tableName, string fileds, string swhere, string orderby)
        {
            string where = "StatusCode=1";
            where += " and " + swhere;
            if (swhere.IsNotNullOrEmpty())
                where = " where " + where;
            return instance._sqlsugar.Db.Ado.GetDataTable($"select {fileds} from {tableName} {where} order by {orderby}");
        }

        private static DataTable GetDataList(string tableName, string fileds, string swhere, string orderby, int page, int pagesize, ref int recount)
        {
            string where = "StatusCode=1";
            where += " and " + swhere;
            if (swhere.IsNotNullOrEmpty())
                where = " where " + where;
            string pagesql = $"select {fileds} from {tableName}  {where} order by {orderby}";
            if (page <= 0)
                page = 1;
            var dt = instance._sqlsugar.GetPageDataList(page, pagesize, pagesql, ref recount);
            return dt;
        }


        /// <summary>
        /// 不分页模式
        /// </summary>
        /// <param name="firstDto"></param>
        /// <param name="swhere"></param>
        /// <returns></returns>
        public static object GetJsonDataByContentDto(ContentModelDto firstDto, string swhere)
        {
            object list = new List<object>();
            DataTable dataTable = GetDataList(firstDto.TableName, firstDto.TableColumnStr, swhere, "OrderId desc");
            List<Dictionary<object, object>> modellist = new List<Dictionary<object, object>>();
            foreach (DataRow item in dataTable.Rows)
            {
                Dictionary<object, object> dict = GetDictionaryByModelColumnList(firstDto, item);
                modellist.Add(dict);
            }
            list = (modellist);
            return list;
        }
        /// <summary>
        /// 不分页模式
        /// </summary>
        /// <param name="firstDto"></param>
        /// <param name="swhere"></param>
        /// <returns></returns>
        public static object GetJsonDataByContentDtoPageDocx(ContentModelDto firstDto, string swhere)
        {
            object list = new List<object>();
            DataTable dataTable = GetDataList(firstDto.TableName, firstDto.TableColumnStr, swhere, "OrderId desc");
            List<Dictionary<object, object>> modellist = new List<Dictionary<object, object>>();
            foreach (DataRow item in dataTable.Rows)
            {
                Dictionary<object, object> dict = GetDictionaryByModelColumnList(firstDto, item, 2);
                modellist.Add(dict);
            }
            list = modellist;
            return list;
        }
        /// <summary>
        /// 分页模式
        /// </summary>
        /// <param name="firstDto"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="swhere"></param>
        /// <param name="recount"></param>
        /// <returns></returns>
        public static Dictionary<object, object> GetJsonDataByContentDto(ContentModelDto firstDto, int page, int pagesize, string swhere, ref int recount)
        {
            Dictionary<object, object> list = new Dictionary<object, object>();
            DataTable dataTable = GetDataList(firstDto.TableName, firstDto.TableColumnStr, swhere, "OrderId desc", page, pagesize, ref recount);
            List<Dictionary<object, object>> modellist = new List<Dictionary<object, object>>();
            foreach (DataRow item in dataTable.Rows)
            {
                Dictionary<object, object> dict = GetDictionaryByModelColumnList(firstDto, item);
                modellist.Add(dict);
            }
            list.Add(GetvirtualTableName(firstDto.TableName), modellist);
            return list;
        }
        /// <summary>
        /// 获取字段筛选条件
        /// </summary>
        /// <param name="filedModels"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static string GetExpressionByFiledNameList(List<FiledModel> filedModels, string k)
        {
            string swhere = string.Empty;
            if (filedModels.Count() > 0)
            {
                swhere += " and (";
                for (int i = 0; i < filedModels.Count(); i++)
                {
                    swhere += i == 0 ? "" : " or ";
                    swhere += filedModels[i].RealFiledName + " like '%" + k + "%'";
                }
                swhere += ")";
            }
            return swhere;
        }
        /// <summary>
        /// 分页模式
        /// </summary>
        /// <param name="firstDto"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="swhere"></param>
        /// <param name="recount"></param>
        /// <returns></returns>
        public static Dictionary<object, object> GetJsonDataByContentDtoPageDocx(ContentModelDto firstDto, int page, int pagesize, string swhere, ref int recount)
        {
            Dictionary<object, object> list = new Dictionary<object, object>();
            DataTable dataTable = GetDataList(firstDto.TableName, firstDto.TableColumnStr, swhere, "OrderId desc", page, pagesize, ref recount);
            List<Dictionary<object, object>> modellist = new List<Dictionary<object, object>>();
            foreach (DataRow item in dataTable.Rows)
            {
                Dictionary<object, object> dict = GetDictionaryByModelColumnList(firstDto, item, 2);
                modellist.Add(dict);
            }
            list.Add(GetvirtualTableName(firstDto.TableName), modellist);
            return list;
        }

        /// <summary>
        /// 填充子列表
        /// </summary>
        /// <param name="swhere"></param>
        /// <param name="contentDto"></param>
        /// <returns></returns>
        public static void FillChildData(Dictionary<string, List<Dictionary<object, object>>> childtabledict, string swhere, ContentModelDto contentDto)
        {
            DataTable childdataTable = null;
            foreach (var childmodel in contentDto.childModelDtos)
            {
                childmodel.TableColumnList.Add(new FiledModel("PId", "关联Id", "input"));
                childdataTable = GetDataList(childmodel.TableName, childmodel.TableColumnStr + ",PId", swhere, "OrderId desc");
                var childlist = new List<Dictionary<object, object>>();
                foreach (DataRow item in childdataTable.Rows)
                {
                    Dictionary<object, object> dict = GetDictionaryByModelColumnList(childmodel, item);

                    //if (childmodel.childModelDtos != null)
                    //{
                    //    FillChildData(childtabledict, swhere, childmodel);
                    //}
                    childlist.Add(dict);
                }
                childtabledict.Add(childmodel.TableName, childlist);
            }
        }

        /// <summary>
        /// 循环所需字段并填充字典
        /// </summary>
        /// <param name="contentmodel"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Dictionary<object, object> GetDictionaryByModelColumnList(ContentModelDto contentmodel, DataRow item, int mode = 1)
        {
            Dictionary<object, object> dict = new Dictionary<object, object>();
            var tbname = GetvirtualTableName(contentmodel.TableName);
            foreach (var column in contentmodel.TableColumnList)
            {
                SetupColumnData(item, dict, column.FiledName, column.FiledMode, mode, tbname);
            }
            return dict;
        }
        /// <summary>
        /// 根据控件类型返回格式化后的值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dict"></param>
        /// <param name="column"></param>
        /// <param name="columndesc"></param>
        public static void SetupColumnData(DataRow item, Dictionary<object, object> dict, object column, string columndesc, int mode = 1, string tablename = null)
        {
            //API模式
            if (mode == 1)
            {
                switch (columndesc)
                {
                    case "DateType": dict.Add(column, item[column.ToString()].ToString()); break;
                    case "editor": dict.Add(column, item[column.ToString()].ToHtmlDeconde()); break;
                    case "PicType": dict.Add(column, item[column.ToString()].ToImgUrl()); break;
                    case "MultipleTextType": dict.Add(column, item[column.ToString()].ToString().Replace("\n", "<br>")); break;
                    case "multiimage": dict.Add(column, item[column.ToString()].ToString().ToImgListUrl(mode)); break;
                    case "MutiImgExt": break;
                    default: dict.Add(column, item[column.ToString()]); break;
                }
            }
            else
            {
                switch (columndesc)
                {
                    case "DateType": dict.Add(tablename + column, "<span class='json-string'>" + item[column.ToString()].ToString() + "</span>"); break;
                    case "editor": dict.Add(tablename + column, "<span class='json-string'>" + item[column.ToString()].ToHtmlEnconde().subString(100) + "...后续省略</span>"); break;
                    case "PicType": dict.Add(tablename + column, "<span class='json-url'>" + item[column.ToString()].ToImgUrl() + "</span>"); break;
                    case "MultipleTextType": dict.Add(tablename + column, "<span class='json-string'>" + item[column.ToString()].ToString().ReplaceDefaultStrEncode() + "</span>"); break;
                    case "multiimage": dict.Add(tablename + column, item[column.ToString()].ToString().ToImgListUrl(mode)); break;
                    case "MutiImgExt": break;
                    default: dict.Add(tablename + column, "<span class='json-string'>" + item[column.ToString()] + "</span>"); break;
                }
            }
        }
        /// <summary>
        /// 获取虚拟表名
        /// </summary>
        /// <returns></returns>
        public static string GetvirtualTableName(string tablename)
        {
            if (!tablename.Contains("tbl_normal_"))
                return tablename + "ViewModel";
            return tablename.Split(new string[] { "tbl_normal_" }, StringSplitOptions.RemoveEmptyEntries)[0] + "ViewModel";
        }
        /// <summary>
        /// 获取需展示的表头集合
        /// </summary>
        /// <param name="Id">栏目Id</param>
        /// <returns></returns>
        public static List<ContentModelDto> GetProductTableColumnHashTable(string Id)
        {
            if (!Id.Contains(","))
            {
                var columnCategory = column.Select("Id=" + Id);
                if (columnCategory.Length != 1)
                    return null;
                int modelid = columnCategory[0]["ModelId"].ToInt();
                List<ContentModelDto> contentModelDtos = new List<ContentModelDto>();
                if (new int[] { 0, -1, -3 }.Contains(modelid))
                {
                    return contentModelDtos;
                }
                contentModelDtos.Add(GetModelContentByModelId(modelid));
                return contentModelDtos;
            }
            else
            {
                List<ContentModelDto> contentModelDtos = new List<ContentModelDto>();
                List<int> modelids = new List<int>();
                foreach (var item in Id.Split(','))
                {
                    var columnCategory = column.Select("Id=" + item);
                    if (columnCategory.Length != 1)
                        return null;
                    int modelid = columnCategory[0]["ModelId"].ToInt();
                    if (modelids.Contains(modelid))
                        continue;
                    modelids.Add(modelid);
                    contentModelDtos.Add(GetModelContentByModelId(modelid));
                }
                return contentModelDtos;
            }
            //FillContentModelList(modelid, fileds);
        }

        /// <summary>
        /// 填充内容模型列表
        /// </summary>
        /// <param name="modelid"></param>
        /// <param name="fileds"></param>
        public static void FillContentModelList(int modelid, ContentModelDto fileds)
        {
            //有子列表的情况
            var child = relationlist.Select("ParentModelId=" + modelid);
            if (child.Length > 0)
            {
                List<ContentModelDto> childModelDtos = new List<ContentModelDto>();
                foreach (var item in child)
                {
                    var childs = GetModelContentByModelId(item["ChildModelId"].ToInt());
                    childModelDtos.Add(childs);
                    //FillContentModelList(item["ChildModelId"].ToInt(), childs);
                }
                fileds.childModelDtos = childModelDtos;
            }
        }
        public static string splitstr = "[|]";
        public static List<FiledModel> defaultfileds = new List<FiledModel>
            {
                new FiledModel("Id","编号","input"),
                new FiledModel("ParentId","栏目","input"),
                new FiledModel("Title","标题","input",true),
                new FiledModel("AddTime","添加时间", "date")
            };
        /// <summary>
        /// 获取模型字段列表
        /// </summary>
        /// <param name="modelid">模型ID</param>
        /// <returns></returns>
        public static ContentModelDto GetModelContentByModelId(int modelid)
        {
            var ContentModel = contentlist.Select("Id=" + modelid);
            if (ContentModel.Length != 1)
                return null;
            ContentModelDto fileds = new ContentModelDto();
            fileds.TableName = ContentModel[0]["TableName"].ToString();
            fileds.ModelName = ContentModel[0]["Name"].ToString();

            var filedlist = ContentModelHelper.filedlist.Select("ModelId=" + modelid + " and IsApiField=1");
            List<FiledModel> filedlists = defaultfileds.ToList();
            string column_str = "Id,ParentId,Title,AddTime";
            if (filedlist.Length > 0)
            {
                foreach (DataRow dr in filedlist)
                {
                    FiledModel filedModel = new FiledModel();
                    filedModel.FiledDesc = dr["Name"].ToString();
                    filedModel.FiledMode = dr["FieldType"].ToString();
                    filedModel.FiledName = dr["FieldName"].ToString();
                    filedModel.RealFiledName = dr["FieldName"].ToString();

                    if (!dr["ApiName"].ToString().IsEmpty())
                    {
                        filedModel.FiledName = dr["ApiName"].ToString();
                        column_str += "," + dr["FieldName"] + " as [" + dr["ApiName"] + "]";
                    }
                    else
                    {
                        column_str += "," + dr["FieldName"];
                    }
                    filedModel.IsSearch = dr["IsSearch"].ToInt() == 0 ? false : true;
                    filedlists.Add(filedModel);
                }
                if (filedlists.Any(m => m.FiledMode == "MutiImgSelect"))
                {
                    filedlists.Add(new FiledModel("imglist_title", "图集标题", "MutiImgExt"));
                    filedlists.Add(new FiledModel("imglist_img", "图集图片", "MutiImgExt"));
                    filedlists.Add(new FiledModel("imglist_link", "图集链接", "MutiImgExt"));
                }
            }
            fileds.TableColumnStr = column_str;
            fileds.TableColumnList = filedlists;
            return fileds;
        }
        #endregion


        public static ColumnJsonDocxDto GetContentJsonByExpression(InputJsondocxDto inputJsondocxDto)
        {
            ColumnJsonDocxDto columnJsonDocxDto;
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

            if (inputJsondocxDto.PId != 0)
                urlext += "&PId=" + inputJsondocxDto.PId;

            Dictionary<object, List<FiledModel>> full_fileds = new Dictionary<object, List<FiledModel>>();
            columnJsonDocxDto = new ColumnJsonDocxDto();
            #region 拼接条件
            //columnId = "61,62,63";
            int recount = 0;
            string swhere = $"IsHide=0  and StatusCode=1 and PublishTime<='{Clock.Now}'";
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
            if (contentDto == null)
            {
                jsonstr += "没有内容";
                columnJsonDocxDto.full_fileds = full_fileds;
                columnJsonDocxDto.JsonStr = jsonstr;
                columnJsonDocxDto.urlext = urlext;
                return columnJsonDocxDto;
            }
            #endregion
            if (contentDto.Count == 0)
            {
                jsonstr += "没有内容";
                columnJsonDocxDto.full_fileds = full_fileds;
                columnJsonDocxDto.JsonStr = jsonstr;
                columnJsonDocxDto.urlext = urlext;
                return columnJsonDocxDto;
            }

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
                        {"img_src","img_src" },
                        {"img_title","img_title" },
                        {"img_content","img_content" },
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
                        {"img_src","img_src" },
                        {"img_title","img_title" },
                        {"img_content","img_content" },
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
            columnJsonDocxDto.full_fileds = full_fileds;
            columnJsonDocxDto.JsonStr = jsonstr;
            columnJsonDocxDto.urlext = urlext;
            return columnJsonDocxDto;
        }
    }

}
