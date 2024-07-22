using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.Excel;
using Flex.Core.Config;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.System.ContentModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Flex.Application.SignalRBus.Services
{
    public class ExportBackgroundService : BackgroundService
    {
        private readonly IHubNotificationService _hubContext;
        private readonly ILogger<ExportBackgroundService> _logger;
        private readonly IConcurrentQueue<ContentPageListParamDto> _exportQueue;
        private IMessageServices _messageServices;
        private IColumnContentServices _contentServices;
        IWebHostEnvironment _env;
        string basePath = string.Empty;

        /// <summary>
        /// 文件储存位置
        /// </summary>
        private string exportsFolder = CurrentSiteInfo.SiteUploadPath + $"/Excel/exports/{DateTime.Now.ToDefaultDateTimeStr()}";

        public ExportBackgroundService(
              IHubNotificationService hubContext
            , ILogger<ExportBackgroundService> logger
            , IConcurrentQueue<ContentPageListParamDto> exportQueue
            , IWebHostEnvironment env
            , IMessageServices messageServices
            , IColumnContentServices contentServices
            )
        {
            _hubContext = hubContext;
            _logger = logger;
            _exportQueue = exportQueue;
            _env = env;
            basePath = _env.WebRootPath + exportsFolder;
            _messageServices = messageServices;
            _contentServices = contentServices;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //这里可以使用队列或其他方式来管理导出请求
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("正在处理导出任务...");
                try
                {
                    var exportRequest = await _exportQueue.DequeueAsync(stoppingToken);
                    if (exportRequest != null)
                    {
                        //_logger.LogInformation("开始导出");
                        await ExportDataTableInChunks(exportRequest);
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
        public async Task ExportDataTableInChunks(ContentPageListParamDto requestmodel)
        {
            int pageSize = 10000; // 每次处理的行数
            int pageIndex = 1; // 分页索引
            bool moreData = true;
            int fileIndex = 0;
            string fileName = string.Empty;
            string msgcontent = string.Empty;

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            while (moreData)
            {
                requestmodel.limit = pageSize;
                requestmodel.page = pageIndex;
                var resultmodel = await _contentServices.GetExportExcelDataTableAsync(requestmodel);
                if (!resultmodel.IsSuccess)
                {
                    await _hubContext.SendError(requestmodel.UserId, resultmodel.Detail);
                    await Task.CompletedTask;
                    return;
                }

                DataTable table = resultmodel.Content.result;
                List<FiledModel> fileModes = resultmodel.Content.filedModels;
                fileName = string.IsNullOrEmpty(fileName) ? resultmodel.Content.ExcelName : fileName;

                if (table.Rows.Count < pageSize)
                {
                    moreData = false;
                }
                fileIndex++;
                string currentfileName = $"{fileName}_{fileIndex}.xlsx"; // 动态生成文件名
                using (var stream = ExcelOperate.SimpleExportToSpreadsheet(table, fileModes))
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

                pageIndex++;
            }
            msgcontent = $"<p>本次导出为{fileIndex}个文件，请自行点击下载：</p><br/>" + msgcontent;
            //发送消息给自己
            await _messageServices.SendExportMsg("导出文件", msgcontent, _hubContext.GetClaims(requestmodel.UserId));
            await _hubContext.NotifyCompletion(requestmodel.UserId, "导出任务完成");
        }


    }
}
