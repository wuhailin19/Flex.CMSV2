using Dapper;
using Flex.Application.Contracts.Exceptions;
using Flex.Core.Extensions.CommonExtensions;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Enums.LogLevel;
using Flex.Domain.WhiteFileds;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices
    {
        /// <summary>
        /// 修改数据内容
        /// </summary>
        /// <param name="table">前端传入字段</param>
        /// <param name="IsReview">当前修改模式是否为审核</param>
        /// <param name="white_fileds">本次修改的字段白名单</param>
        /// <param name="IsCancelReview">取消审批确认参数</param>
        /// <returns></returns>
        public async Task<ProblemDetails<int>> Update(
            Hashtable table
            , bool IsReview = false
            , List<string> white_fileds = null
            , bool IsCancelReview = false)
        {
            if (table.Count == 0)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNeedReview.GetEnumDescription());

            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());

            var filedmodel = await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId);
            ClearNotUseFields(table, white_fileds, filedmodel);

            InitUpdateTable(table);
            //当前表中字段用于生成修改副本
            var fileds = ColumnContentUpdateFiledConfig.updatehistoryFields.ToList();
            foreach (var item in filedmodel)
            {
                fileds.Add(item.FieldName);
            }
            return await UpdateContentCore(table, contentmodel, fileds, column);
        }

        private static void ClearNotUseFields(Hashtable table, List<string> white_fileds, IList<sysField> filedmodel)
        {
            var keysToRemove = new List<object>();
            if (white_fileds.IsNullOrEmpty())
                white_fileds = ColumnContentUpdateFiledConfig.defaultFields.ToList();
            foreach (var item in table.Keys)
            {
                if (white_fileds.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    continue;
                if (!filedmodel.Any(m => m.FieldName.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            foreach (var key in keysToRemove)
                table.Remove(key);
        }

        /// <summary>
        /// 审核后修改的内容
        /// </summary>
        /// <param name="table"></param>
        /// <param name="IsReview"></param>
        /// <param name="IsCancelReview"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<int>> UpdateReviewContent(
            Hashtable table,
            bool IsReview = true,
            bool IsCancelReview = false)
        {
            var whitefields = ColumnContentUpdateFiledConfig.reviewContentFields;
            if (table.Count == 0)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());

            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            if (IsCancelReview)
            {

                DynamicParameters parameters = new DynamicParameters();
                Dictionary<string, object> dict = new Dictionary<string, object>
                    {
                        { "Id", table["Id"] },
                        { "ParentId",  table["ParentId"] }
                    };
                string swhere = string.Empty;
                _sqlTableServices.InitDapperColumnContentSwheresql(ref swhere, ref parameters, dict);


                var result = (await _dapperDBContext.GetDynamicAsync("select ReviewAddUser from " + contentmodel.TableName + " where" + swhere, parameters)).FirstOrDefault();
                if (result == null)
                    return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
                if (result.ReviewAddUser != _claims.UserId && !_claims.IsSystem)
                {
                    return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
                }
            }
            var keysToRemove = new List<object>();

            foreach (var item in table.Keys)
            {
                if (!whitefields.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            foreach (var key in keysToRemove)
                table.Remove(key);
            return await UpdateContentCore(table, contentmodel, null, column);
        }

        /// <summary>
        /// 简单修改内容不留存备份
        /// </summary>
        /// <param name="table"></param>
        /// <param name="IsReview"></param>
        /// <param name="whitefields"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<int>> SimpleUpdateContent(
            Hashtable table,
            bool IsReview = true,
            List<string> whitefields = null)
        {
            whitefields = whitefields ?? ColumnContentUpdateFiledConfig.simpleUpdateFields;
            if (table.Count == 0)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (table.GetValue("Ids").ToString().IsNullOrEmpty())
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (table.ContainsKey("OrderId"))
            {
                table.SetValue("OrderId", table["OrderId"].ToIntAndThrowException());
            }
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());

            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            var keysToRemove = new List<object>();
            foreach (var item in table.Keys)
            {
                if (!whitefields.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            foreach (var key in keysToRemove)
                table.Remove(key);

            return await UpdateContentCore(table, contentmodel, null, column);
        }

        private async Task<ProblemDetails<int>> UpdateContentCore(Hashtable table, SysContentModel contentmodel, List<string> fileds, SysColumn column)
        {
            StringBuilder builder = new StringBuilder();
            SqlSugar.SugarParameter[] parameters;
            table["Id"] = table["Id"].ToInt();
            table["ParentId"] = table["ParentId"].ToInt();
            if (fileds != null)
            {
                //创建历史副本
                parameters = new SqlSugar.SugarParameter[] {
                    new SqlSugar.SugarParameter("@LastEditUser",table["LastEditUser"]),
                    new SqlSugar.SugarParameter("@LastEditUserName",table["LastEditUserName"]),
                    new SqlSugar.SugarParameter("@LastEditDate",table["LastEditDate"]),
                    new SqlSugar.SugarParameter("@Id",table["Id"].ToInt()),
                };
                var createcopysql = _sqlTableServices.CreateInsertCopyContentSqlString(fileds, contentmodel.TableName);
                _sqlsugar.Db.Ado.ExecuteCommand(createcopysql.ToString(), parameters);
            }

            parameters = new SqlSugar.SugarParameter[] { };
            builder = _sqlTableServices.CreateSqlsugarUpdateSqlString(table, contentmodel.TableName, out parameters);
            try
            {
                var result = _sqlsugar.Db.Ado.ExecuteCommand(builder.ToString(), parameters);
                if (result > 0)
                {
                    if (table.ContainsKey("Title"))
                    {
                        await _logServices.AddContentLog(SystemLogLevel.Warning, $"修改栏目【{column.Name}】数据【{table.GetValue("Title")}】", $"修改");
                    }
                    return Problem<int>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
                }
                return Problem<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<int>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
    }
}
