using Flex.Domain.Dtos.System.ContentModel;
using Flex.SqlSugarFactory.Seed;
using ShardingCore.Extensions;
using SqlSugar;
using System.Data;

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
                    var columnCategory = instance._sqlsugar.Db.Ado.GetDataTable("select ModelId,Id from tbl_core_column where StatusCode=1");
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
                    var ContentModel = instance._sqlsugar.Db.Ado.GetDataTable("select Id,TableName,Name from tbl_core_contentmodel where StatusCode=1");
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
                    var filedlist = instance._sqlsugar.Db.Ado.GetDataTable("select Name,FieldName,ModelId,FieldType,ApiName,IsSearch from tbl_core_field where StatusCode=1 order by OrderId asc");
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
                where = " where " + swhere;
            return instance._sqlsugar.Db.Ado.GetDataTable($"select {fileds} from {tableName} where {where} order by {orderby}");
        }

        private static DataTable GetDataList(string tableName, string fileds, string swhere, string orderby, int page, int pagesize, ref int recount)
        {
            string where = "StatusCode=1";
            where += " and " + swhere;
            string pagesql = $"select {fileds} from {tableName} where {where} order by {orderby} offset";
            if (page <= 0)
                page = 1;
            pagesql += $" ({(page - 1) * pagesize}) rows fetch next {pagesize} rows only";
            recount = instance._sqlsugar.Db.Ado.SqlQuerySingle<int>($"select count(1) from {tableName} where StatusCode=1");
            return instance._sqlsugar.Db.Ado.GetDataTable(pagesql);
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
                childmodel.TableColumnList.Add(new FiledModel("PId", "关联Id", "TextType"));
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
            foreach (var column in contentmodel.TableColumnList)
            {
                SetupColumnData(item, dict, column.FiledName, column.FiledMode, mode);
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
        public static void SetupColumnData(DataRow item, Dictionary<object, object> dict, object column, string columndesc, int mode = 1)
        {
            //API模式
            if (mode == 1)
            {
                switch (columndesc)
                {
                    case "DateType": dict.Add(column, item[column.ToString()].ToString()); break;
                    case "editor": dict.Add(column, item[column.ToString()].ToHtmlDeconde()); break;
                    case "PicType": dict.Add(column, item[column.ToString()].ToImgUrl()); break;
                    case "MultipleTextType": dict.Add(column, item[column.ToString()].ToString().ReplaceDefaultStr()); break;
                    case "MutiImgSelect": dict.Add(column, item[column.ToString()].ToString().ToImgListUrl(mode)); break;
                    case "MutiImgExt": break;
                    default: dict.Add(column, item[column.ToString()]); break;
                }
            }
            else
            {
                switch (columndesc)
                {
                    case "DateType": dict.Add(column, "<span class='json-string'>" + item[column.ToString()].ToString() + "</span>"); break;
                    case "editor": dict.Add(column, "<span class='json-string'>" + item[column.ToString()].ToHtmlEnconde().subString(100) + "...后续省略</span>"); break;
                    case "PicType": dict.Add(column, "<span class='json-url'>" + item[column.ToString()].ToImgUrl() + "</span>"); break;
                    case "MultipleTextType": dict.Add(column, "<span class='json-string'>" + item[column.ToString()].ToString().ReplaceDefaultStrEncode() + "</span>"); break;
                    case "MutiImgSelect": dict.Add(column, item[column.ToString()].ToString().ToImgListUrl(mode)); break;
                    case "MutiImgExt": break;
                    default: dict.Add(column, "<span class='json-string'>" + item[column.ToString()] + "</span>"); break;
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

            var filedlist = ContentModelHelper.filedlist.Select("ModelId=" + modelid);
            if (filedlist.Length > 0)
            {
                List<FiledModel> hashtable = new List<FiledModel>
            {
                new FiledModel("Id","编号","TextType"),
                new FiledModel("ParentId","栏目","TextType"),
                new FiledModel("Title","标题","TextType",true),
                new FiledModel("AddTime","添加时间", "DateType")
            };
                string column_str = "Id,ParentId,Title,AddTime";
                foreach (DataRow dr in filedlist)
                {
                    FiledModel filedModel = new FiledModel();
                    filedModel.FiledDesc = dr["Name"].ToString();
                    filedModel.FiledMode = dr["FieldType"].ToString();
                    filedModel.FiledName = dr["FieldName"].ToString();
                    filedModel.RealFiledName = dr["FieldName"].ToString();
                    column_str += "," + dr["FieldName"];

                    if (!dr["ApiName"].ToString().IsEmpty())
                    {
                        filedModel.FiledName = dr["ApiName"].ToString();
                        column_str += "," + dr["FieldName"] + " as [" + dr["ApiName"] + "]";
                    }
                    filedModel.IsSearch = dr["IsSearch"].ToInt() == 0 ? false : true;
                    //switch (filedModel.FiledMode)
                    //{
                    //    case "MultipleTextType":
                    //        filedModel.IsSearch = true;
                    //        break;
                    //    case "Editor":
                    //        filedModel.IsSearch = true;
                    //        break;
                    //}
                    hashtable.Add(filedModel);
                }
                if (hashtable.Any(m => m.FiledMode == "MutiImgSelect"))
                {
                    hashtable.Add(new FiledModel("imglist_title", "图集标题", "MutiImgExt"));
                    hashtable.Add(new FiledModel("imglist_img", "图集图片", "MutiImgExt"));
                    hashtable.Add(new FiledModel("imglist_link", "图集链接", "MutiImgExt"));
                }
                fileds.TableColumnStr = column_str;
                fileds.TableColumnList = hashtable;
            }
            return fileds;
        }
        #endregion

    }

}
