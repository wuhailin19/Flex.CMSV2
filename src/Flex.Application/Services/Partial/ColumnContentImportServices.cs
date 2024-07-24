using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Application.Excel;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Flex.Domain.WhiteFileds;
using SqlSugar;
using System.Data;
using System.Text;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices
    {

        public async Task<ProblemDetails<ImportResultModel>> ImportExcelToModel(ImportRequestModel uploadExcelFileDto)
        {
            var dt=new DataTable();
            try
            {
                using (var memoryStream = new MemoryStream(uploadExcelFileDto.FileContent))
                {
                    dt = ExcelOperate.ImportExcelToDataTableFromStream(memoryStream);
                }
            }
            catch (Exception ex)
            {
                return Problem<ImportResultModel>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            }
            
            if (dt.Rows.Count == 0)
            {
                return Problem<ImportResultModel>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            }

            var model = await GetFieldContentAndColumnByColumnId(uploadExcelFileDto.ParentId, uploadExcelFileDto.ModelId);
            var statuscode = 1;
            //栏目需审核则默认为待审核状态
            if (model.Column.ReviewMode.ToInt() != 0)
                statuscode = 5;
            if (model.ContentModel == null)
                return Problem<ImportResultModel>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            var filedmodel = model.Field;
            StringBuilder insertsbr = new StringBuilder();
            var white_fileds = ColumnContentUpdateFiledConfig.defaultImportFields.ToList();
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

            string orderSql = _sqlTableServices.GetNextOrderIdDapperSqlString(model.ContentModel.TableName);
            var orderId = _sqlsugar.Db.Ado.GetDataTable(orderSql)?.Rows[0][0].ToInt() ?? 0;


            return Problem(HttpStatusCode.OK, new ImportResultModel
            {
                dt = dt,
                statuscode = statuscode,
                OrderId = orderId,
                TableName= model.ContentModel.TableName
            }, 
            ErrorCodes.DataInsertSuccess.GetEnumDescription());
        }
    }
}
