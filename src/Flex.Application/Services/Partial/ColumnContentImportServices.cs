using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices;
using Flex.Application.Excel;
using Flex.Core.Config;
using Flex.Core.Framework.Enum;
using Flex.Domain.Dtos.System.Upload;
using Flex.Domain.WhiteFileds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices
    {

        public async Task<ProblemDetails<int>> ImportExcelToModel(UploadExcelFileDto uploadExcelFileDto)
        {
            if (uploadExcelFileDto.file == null)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.UploadTypeDenied.GetEnumDescription());
            var file = uploadExcelFileDto.file[0];
            if (!FileCheckHelper.IsAllowedExcelExtension(file))
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.UploadTypeDenied.GetEnumDescription());
            uploadExcelFileDto.UserId = _claims.UserId;
            using var stream = file.OpenReadStream();
            var dt = ExcelOperate.ImportExcelToDataTableFromStream(stream);

            if (dt.Rows.Count == 0)
            {
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            }

            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == uploadExcelFileDto.ParentId);
            var statuscode = 1;
            //栏目需审核则默认为待审核状态
            if (column.ReviewMode.ToInt() != 0)
                statuscode = 5;
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == uploadExcelFileDto.ModelId);
            if (contentmodel == null)
                return Problem(HttpStatusCode.BadRequest, 0, ErrorCodes.DataInsertError.GetEnumDescription());
            var filedmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == contentmodel.Id)).ToList();
            string orderSql = _sqlTableServices.GetNextOrderIdDapperSqlString(contentmodel.TableName);
            var orderId = _sqlsugar.Db.Ado.GetDataTable(orderSql)?.Rows[0][0].ToInt() ?? 0;

            StringBuilder insertsbr = new StringBuilder();

            var white_fileds = ColumnContentUpdateFiledConfig.defaultFields.ToList();

            var timelist = new List<string>();
            //失败的数据
            var errorlist = new List<Hashtable>();

            var keysToRemove = new List<DataColumn>();


            foreach (DataColumn item in dt.Columns)
            {
                //如果传入的字段对应字典包含此字段，则加入重命名行列
                if (uploadExcelFileDto.FieldDict.Any(m => m.field == item.ColumnName))
                {
                    item.ColumnName = uploadExcelFileDto.FieldDict.Where(m => m.field == item.ColumnName).FirstOrDefault()?.value ?? item.ColumnName;
                }
                if (white_fileds.Any(m => m.Equals(item.ColumnName, StringComparison.OrdinalIgnoreCase)))
                    continue;

                if (!filedmodel.Any(m => m.FieldName.Equals(item.ColumnName, StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            //移除多余的列和未在模型中的列
            foreach (var item in keysToRemove)
            {
                dt.Columns.Remove(item);
            }
            foreach (DataRow dr in dt.Rows)
            {
                Hashtable hashtable = new Hashtable();
                foreach (DataColumn item in dt.Columns)
                {
                    //pgSQL情况
                    switch (DataBaseConfig.dataBase)
                    {
                        case DataBaseType.PgSql:
                            if (dr[item.ColumnName].ToString().IsTime())
                            {
                                dr[item.ColumnName] = dr[item.ColumnName].ToString().ToUtcTime();
                            }
                            break;
                        default:
                            break;
                    }
                    hashtable.Add(item.ColumnName, dr[item.ColumnName]);
                }
                SugarParameter[] parameters = [];
                hashtable.Add("StatusCode", statuscode);
                hashtable.Add("ParentId", uploadExcelFileDto.ParentId);
                InitCreateTable(hashtable);
                var insertsql = _sqlTableServices.CreateSqlsugarInsertSqlString(hashtable, contentmodel.TableName, orderId, out parameters);
                orderId++;
                var result = _sqlsugar.Db.Ado.GetScalar(insertsql.ToString(), parameters).ToInt();
                if (result <= 0)
                {
                    errorlist.Add(hashtable);
                }
            }
            if (errorlist.Count > 0)
            {
                var tablestr =new StringBuilder("<table>");
                tablestr.Append("<th>");
                foreach (DataColumn item in dt.Columns)
                {
                    tablestr.Append($"<td>{item.Caption}<td>");
                }
                tablestr.Append("</th>");
                tablestr.Append("<tbody>");
                foreach (var item in errorlist)
                {
                    tablestr.Append("<tr>");
                    foreach (var key in item.Keys)
                    {
                        tablestr.Append($"<td>{item[key]}<td>");
                    }
                    tablestr.Append("</tr>");
                }
                tablestr.Append("</tbody>");
                tablestr.Append("</table>");
                var _messageServices = _serviceProvider.GetService<IMessageServices>();
                await _messageServices.SendNormalMsg("导入失败", "以下数据导入失败：<br>"+ tablestr);
                return Problem<int>(HttpStatusCode.BadGateway, $"有{errorlist.Count}条导入失败的数据");
            }
            return Problem<int>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
        }
    }
}
