using Flex.Domain.Dtos.System.ContentModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Flex.Application.Excel
{
	public class ExcelOperate
    {
		public static MemoryStream SimpleExportToSpreadsheet(DataTable table, List<FiledModel> fileModes)
		{
			IWorkbook workbook = new XSSFWorkbook();
			ISheet sheet = workbook.CreateSheet("Sheet1");

			// 设置表头
			IRow headerRow = sheet.CreateRow(0);
			for (int i = 0; i < fileModes.Count; i++)
			{
				ICell cell = headerRow.CreateCell(i);
				cell.SetCellValue(fileModes[i].FiledDesc + $"-{fileModes[i].FiledName}");
				ICellStyle headerStyle = workbook.CreateCellStyle();
				headerStyle.Alignment = HorizontalAlignment.Center;
				headerStyle.VerticalAlignment = VerticalAlignment.Center;
				headerStyle.BorderTop = BorderStyle.Thin;
				headerStyle.BorderBottom = BorderStyle.Thin;
				headerStyle.BorderLeft = BorderStyle.Thin;
				headerStyle.BorderRight = BorderStyle.Thin;
				cell.CellStyle = headerStyle;
			}

			// 填充数据
			for (int i = 0; i < table.Rows.Count; i++)
			{
				IRow row = sheet.CreateRow(i + 1);
				for (int j = 0; j < fileModes.Count; j++)
				{
					ICell cell = row.CreateCell(j);
					cell.SetCellValue(table.Rows[i][fileModes[j].FiledName]?.ToString());
					ICellStyle cellStyle = workbook.CreateCellStyle();
					cellStyle.Alignment = HorizontalAlignment.Center;
					cellStyle.VerticalAlignment = VerticalAlignment.Center;
					cellStyle.BorderTop = BorderStyle.Thin;
					cellStyle.BorderBottom = BorderStyle.Thin;
					cellStyle.BorderLeft = BorderStyle.Thin;
					cellStyle.BorderRight = BorderStyle.Thin;

					// 如果是日期时间列，设置单元格格式
					if (fileModes[j].FiledMode == "date")
					{
						IDataFormat format = workbook.CreateDataFormat();
						cellStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");
					}

					cell.CellStyle = cellStyle;
				}
			}

			// 返回文件流
			var stream = new MemoryStream();
			workbook.Write(stream);
			stream.Position = 0;
			workbook.Close();

			return stream;
		}

		public static DataTable ImportExcelToDataTableFromStream(Stream stream, bool isXlsx = true)
		{
			DataTable dataTable = new DataTable();
			IWorkbook workbook = isXlsx ? (IWorkbook)new XSSFWorkbook(stream) : new HSSFWorkbook(stream);
			ISheet sheet = workbook.GetSheetAt(0);

			// 获取表格维度
			IRow headerRow = sheet.GetRow(0);
			int colCount = headerRow.LastCellNum;
			int rowCount = sheet.LastRowNum;

			// 添加列
			for (int col = 0; col < colCount; col++)
			{
				dataTable.Columns.Add(headerRow.GetCell(col).ToString());
			}

			// 添加行
			for (int row = 1; row <= rowCount; row++) // Assumes first row is header
			{
				DataRow dataRow = dataTable.NewRow();
				IRow sheetRow = sheet.GetRow(row);

				for (int col = 0; col < colCount; col++)
				{
					dataRow[col] = sheetRow.GetCell(col)?.ToString(); // or .ToString() for raw value
				}

				dataTable.Rows.Add(dataRow);
			}

			return dataTable;
		}

	}
}
