using Flex.Application.Contracts.Exceptions;
using Flex.Dapper;
using Flex.Dapper.Context;
using Flex.Domain;
using Flex.Domain.Base;
using Flex.Domain.Dtos.Column;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using Microsoft.Data.SqlClient;
using System.Text;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.HtmlHelper;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Flex.Application.Services
{
    public class ColumnContentServices : BaseService, IColumnContentServices
    {
        MyDBContext _dapperDBContext;

        //默认加载字段
        private const string defaultFields = "IsTop,IsRecommend,IsHot,IsShow,IsColor" +
            ",Title,Id,AddTime,StatusCode,AddUserName,LastEditUserName,OrderId,ParentId,";
        public ColumnContentServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, MyDBContext dapperDBContext)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _dapperDBContext = dapperDBContext;
        }
        public async Task<IEnumerable<ModelTools<ColumnContentDto>>> GetTableThs(int ParentId)
        {
            var tableths = ModelTools<ColumnContentDto>.getColumnDescList();
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId && m.ShowInTable == true)).ToList();
            foreach (var item in fieldmodel)
            {
                tableths.Add(new ModelTools<ColumnContentDto>()
                {
                    title = item.Name,
                    sort = false,
                    align = "center",
                    maxWidth = "100",
                    field = item.FieldName
                });
            }
            return tableths;
        }
        public async Task<Page> ListAsync(int pageindex, int pagesize, int ParentId)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if(contentmodel==null)
                return new Page();
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId)).ToList();
            string filed = defaultFields;
            foreach (var item in fieldmodel)
            {
                filed += item.FieldName + ",";
            }
            filed = filed.TrimEnd(',');
            var result = await _dapperDBContext.PageAsync(pageindex, pagesize, "select " + filed + " from " + contentmodel.TableName + "");
            result.Items.Each(item =>
            {
                item.StatusColor = ((StatusCode)item.StatusCode).GetEnumColorDescription();
                item.StatusCode = ((StatusCode)item.StatusCode).GetEnumDescription();
            });
            return result;
        }

        public async Task<dynamic> GetContentById(int ParentId, int Id)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return default;
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId)).ToList();
            string filed = defaultFields;
            List<string> editors = new List<string>();
            foreach (var item in fieldmodel)
            {
                filed += item.FieldName + ",";
                if (item.FieldType == nameof(Editor))
                    editors.Add(item.FieldName);
            }
            filed = filed.TrimEnd(',');
            var result = await _dapperDBContext.GetDynamicAsync("select " + filed + " from " + contentmodel.TableName + " where Id=" + Id);

            #region 废弃
            //UpdateContentDto updateContentDto = new UpdateContentDto();
            //if (result.Count() != 1)
            //    return string.Empty;
            //var currentmodel = result.FirstOrDefault();
            //var filedlist = filed.ToList();
            //Dictionary<object, object> normalItems = new Dictionary<object, object>();
            //Dictionary<object, object> editorItems = new Dictionary<object, object>();

            //foreach (KeyValuePair<string, object> col in currentmodel)
            //{
            //    if (!editors.Contains(col.Key))
            //        normalItems.Add(col.Key, col.Value);
            //    else
            //        editorItems.Add(col.Key, col.Value);
            //}

            //updateContentDto.normalItem = normalItems;
            //updateContentDto.editorItem = editorItems;
            #endregion

            return result.FirstOrDefault();
        }

        public async Task<ProblemDetails<string>> GetFormHtml(int ParentId)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "没有选择有效模型");
            return new ProblemDetails<string>(HttpStatusCode.OK, contentmodel.FormHtmlString);
        }
        private void InitTable(Hashtable table)
        {
            table["AddUser"] = _claims.UserId;
            table["AddUserName"] = _claims.UserName;
            table["LastEditUser"] = _claims.UserId;
            table["LastEditUserName"] = _claims.UserName;
            table["LastEditDate"] = Clock.Now;
            table["OrderId"] = 0;
        }
        public async Task<ProblemDetails<string>> Add(Hashtable table)
        {
            InitTable(table);
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into " + contentmodel.TableName + " (");
            string key = "";
            string keyvar = "";
            foreach (DictionaryEntry myDE in table)
            {
                key += "[" + myDE.Key.ToString() + "],";
                keyvar += "@" + myDE.Key.ToString() + ",";

            }
            builder.Append(key.Substring(0, key.Length - 1) + " ) values(" + keyvar.Substring(0, keyvar.Length - 1) + " )");
            SqlParameter[] commandParameters = new SqlParameter[table.Count];
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num] = new SqlParameter("@" + myDE.Key.ToString(), myDE.Value);
                num++;
            }
            try
            {
                var result = _unitOfWork.ExecuteSqlCommand(builder.ToString(), commandParameters);

                if (result > 0)
                {
                    await _unitOfWork.SaveChangesAsync();
                    return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
                }
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
        }
        public async Task<ProblemDetails<string>> Update(Hashtable table)
        {
            InitTable(table);
            int Id = table["Id"].ToInt();
            table.Remove("Id");
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            StringBuilder builder = new StringBuilder();
            builder.Append("Update " + contentmodel.TableName + " set ");
            string keyvar = "";
            foreach (DictionaryEntry myDE in table)
            {
                keyvar += "[" + myDE.Key.ToString() + "]" + "=" + "@" + myDE.Key.ToString() + ",";

            }
            builder.Append(keyvar.Substring(0, keyvar.Length - 1));
            SqlParameter[] commandParameters = new SqlParameter[table.Count];
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num] = new SqlParameter("@" + myDE.Key.ToString(), myDE.Value);
                num++;
            }
            builder.Append(" where Id=" + Id);
            try
            {
                var result = _unitOfWork.ExecuteSqlCommand(builder.ToString(), commandParameters);

                if (result > 0)
                {
                    await _unitOfWork.SaveChangesAsync();
                    return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
                }
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
        }
    }
}
