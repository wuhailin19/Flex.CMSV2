using Flex.Application.Contracts.ISignalRBus;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.SignalRBus.Hubs;
using Flex.Core.Config;
using Flex.Domain.Dtos.System.ContentModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace Flex.Application.SignalRBus.Services
{
    public class ExportBackgroundService : BackgroundService
    {
        private readonly IHubContext<ExportHub> _hubContext;
        private readonly ILogger<ExportBackgroundService> _logger;
        private readonly IConcurrentQueue<ExportRequest> _exportQueue;
        private IMessageServices _messageServices;
        IWebHostEnvironment _env;
        string basePath = string.Empty;

        /// <summary>
        /// 文件储存位置
        /// </summary>
        private string exportsFolder = CurrentSiteInfo.SiteUploadPath + $"/Exports/{DateTime.Now.ToDefaultDateTimeStr()}";

        public ExportBackgroundService(
              IHubContext<ExportHub> hubContext
            , ILogger<ExportBackgroundService> logger
            , IConcurrentQueue<ExportRequest> exportQueue
            , IWebHostEnvironment env
            , IMessageServices messageServices
            )
        {
            _hubContext = hubContext;
            _logger = logger;
            _exportQueue = exportQueue;
            _env = env;
            basePath = _env.WebRootPath + exportsFolder;
            _messageServices = messageServices;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //这里可以使用队列或其他方式来管理导出请求
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("正在处理任务...");
                try
                {
                    var exportRequest = await _exportQueue.DequeueAsync(stoppingToken);
                    if (exportRequest != null)
                    {
                        //_logger.LogInformation("开始导出");
                        await ExportDataTableInChunks(exportRequest, _hubContext);
                    }
                    await Task.Delay(1000, stoppingToken); // 等待一段时间再检查队列
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Format());
                }
            }
            _logger.LogInformation("任务已取消");
        }
        public async Task ExportDataTableInChunks(ExportRequest exportRequest, IHubContext<ExportHub> hubContext)
        {
            DataTable table = exportRequest.table;
            List<FiledModel> fileModes = exportRequest.fileModes;
            string fileName = exportRequest.FileName;
            int totalRows = table.Rows.Count;
            int fileIndex = 1;
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            string msgcontent = string.Empty;
            for (int startRow = 0; startRow < totalRows; startRow += 10000) // 每 10,000 行创建一个新文件
            {
                // 创建当前批次的 DataTable
                DataTable chunkTable = table.Clone(); // 克隆表结构
                for (int i = startRow; i < Math.Min(startRow + 10000, totalRows); i++)
                {
                    chunkTable.ImportRow(table.Rows[i]); // 导入行
                }

                string currentfileName = $"{fileName}_{fileIndex++}.xlsx"; // 动态生成文件名
                using (var stream = SimpleExportToSpreadsheet(chunkTable, fileModes))
                {
                    var filePath = Path.Combine(basePath, currentfileName);
                    _logger.LogInformation("导出路径：" + filePath);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream); // 将流写入文件
                    }
                    //汇总文件列表
                    msgcontent += $"<a href=\"{Path.Combine(exportsFolder, currentfileName)}\" title=\"点击下载\" class=\"exceldownload\">" +
                        $"<span class=\"excelicon\">" +
                        $"<img src=\"/scripts/layui/module/filemanage/ico/xlsx.png\"/>" +
                        $"</span>{currentfileName}</a><br/>";
                }
            }
            //发送消息给自己
            await _messageServices.SendNormalMsg("导出文件", msgcontent, exportRequest.UserId);
            await hubContext.Clients.Client(ExportHub.GetConnectionId(exportRequest.UserId)).SendAsync("ExportCompleted", "导出任务完成");
        }


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
                    worksheet.Cells[1, i + 1].Value = fileModes[i].FiledDesc + "[" + fileModes[i].FiledName + "]";
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
    }
}
