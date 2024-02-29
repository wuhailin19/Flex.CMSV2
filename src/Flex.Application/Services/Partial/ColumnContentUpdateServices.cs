using Dapper;
using Flex.Application.Contracts.Exceptions;
using Flex.Core.Extensions.CommonExtensions;
using Flex.Domain.Dtos.Role;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices : BaseService, IColumnContentServices
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
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());

            var filedmodel = await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId);
            ClearNotUseFields(table, white_fileds, filedmodel);

            InitUpdateTable(table);
            //当前表中字段用于生成修改副本
            var fileds = updatehistoryFields.ToList();
            foreach (var item in filedmodel)
            {
                fileds.Add(item.FieldName);
            }
            return await UpdateContentCore(table, contentmodel, fileds);
        }

        private static void ClearNotUseFields(Hashtable table, List<string> white_fileds, IList<sysField> filedmodel)
        {
            var keysToRemove = new List<object>();
            if (white_fileds.IsNullOrEmpty())
                white_fileds = defaultFields.ToList();
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
        /// <param name="white_fileds"></param>
        /// <param name="IsCancelReview"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<int>> UpdateReviewContent(
            Hashtable table,
            bool IsReview = true,
            bool IsCancelReview = false)
        {
            var whitefields = new List<string> { "ParentId", "Id", "StatusCode", "ReviewStepId", "ReviewAddUser", "MsgGroupId" };
            if (table.Count == 0)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            if (IsCancelReview)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", table["Id"]);
                parameters.Add("@ParentId", table["ParentId"]);
                var result = (await _dapperDBContext.GetDynamicAsync("select ReviewAddUser from " + contentmodel.TableName + " where ParentId=@ParentId and Id=@Id", parameters)).FirstOrDefault();
                if (result == null)
                    return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
                if (result.ReviewAddUser != _claims.UserId && _claims.IsSystem)
                {
                    return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
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
            return await UpdateContentCore(table, contentmodel, null);
        }

        /// <summary>
        /// 修改内容的属性状态或排序号
        /// </summary>
        /// <param name="table"></param>
        /// <param name="IsReview"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<int>> UpdateContentStatus(
            Hashtable table,
            bool IsReview = true)
        {
            var whitefields = new List<string> { "ParentId", "OrderId", "Id", "IsTop", "Ids", "IsRecommend", "IsHot", "IsHide", "IsSilde" };
            if (table.Count == 0)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (table.GetValue("Ids").ToString().IsNullOrEmpty())
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            var keysToRemove = new List<object>();
            foreach (var item in table.Keys)
            {
                if (!whitefields.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            foreach (var key in keysToRemove)
                table.Remove(key);
            return await UpdateContentCore(table, contentmodel, null);
        }

        private async Task<ProblemDetails<int>> UpdateContentCore(Hashtable table, SysContentModel contentmodel, List<string> fileds)
        {
            StringBuilder builder = new StringBuilder();
            SqlParameter[] commandParameters = new SqlParameter[] { };
            if (fileds != null)
            {
                //创建历史副本
                var createcopysql = _sqlTableServices.CreateInsertCopyContentSqlString(table, fileds, contentmodel.TableName, table["Id"].ToInt());
                _unitOfWork.ExecuteSqlCommand(createcopysql.ToString());
            }
            builder = _sqlTableServices.CreateUpdateSqlString(table, contentmodel.TableName, out commandParameters);
            try
            {
                var result = _unitOfWork.ExecuteSqlCommand(builder.ToString(), commandParameters);
                if (result > 0)
                {
                    await _unitOfWork.SaveChangesAsync();

                    return new ProblemDetails<int>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
                }
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
            catch
            {
                throw;
            }
        }
    }
}
