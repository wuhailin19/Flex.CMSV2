using Flex.Domain.Dtos.System.ContentModel;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Flex.Application.Excel
{
    public class ExcelOperate
    {
        /// <summary>
        /// 将DataTable导出为Excel
        /// </summary>
        /// <param name="table">DataTable数据源</param>
        /// <param name="fileModes">字段列表</param>
        public static MemoryStream SimpleExportToSpreadsheet(DataTable table, List<FiledModel> fileModes)
        {
            // 创建Excel包
            using (var package = new ExcelPackage())
            {
                // 添加一个工作表
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                string thsbr = string.Empty;
                for (int i = 0; i < fileModes.Count; i++)
                {
                    // 添加表头
                    worksheet.Cells[1, i + 1].Value = fileModes[i].FiledDesc +$"-{fileModes[i].FiledName}";
                    worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                }

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < fileModes.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = table.Rows[i][fileModes[j].FiledName];
                        worksheet.Cells[i + 2, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[i + 2, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[i + 2, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                        // 如果是日期时间列，设置单元格格式
                        if (fileModes[j].FiledMode == "date")
                        {
                            worksheet.Cells[i + 2, j + 1].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                        }
                    }
                }

                // 返回文件流
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                package.Dispose();

                return stream;
            }
        }

        public static DataTable ImportExcelToDataTableFromStream(Stream stream)
        {
            var dataTable = new DataTable();

            // Ensure the ExcelPackage.LicenseContext is set correctly
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                // Get the first worksheet
                var worksheet = package.Workbook.Worksheets[0];

                // Get the dimensions of the worksheet
                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;

                // Add columns to DataTable
                for (int col = 1; col <= colCount; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, col].Text); // Assumes first row has column names
                }

                // Add rows to DataTable
                for (int row = 2; row <= rowCount; row++) // Assumes first row is header
                {
                    var dataRow = dataTable.NewRow();
                    for (int col = 1; col <= colCount; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Text; // or .Value.ToString() for raw value
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
    }
}
