using Flex.Application.Contracts.ISignalRBus.Enum;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.Excel;
using Flex.Application.SignalRBus.Factory;
using Flex.Core.Config;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
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
        private readonly IConcurrentQueue<ExportRequestModel> _exportQueue;
        private IMessageServices _messageServices;
        private ITaskServices _taskServices;
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
            , IConcurrentQueue<ExportRequestModel> exportQueue
            , IWebHostEnvironment env
            , IMessageServices messageServices
            , IColumnContentServices contentServices
            , ITaskServices taskServices
            )
        {
            _hubContext = hubContext;
            _logger = logger;
            _exportQueue = exportQueue;
            _env = env;
            basePath = _env.WebRootPath + exportsFolder;
            _messageServices = messageServices;
            _contentServices = contentServices;
            _taskServices = taskServices;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("正在处理导出任务...");
            try
            {
                await _exportQueue.ProcessQueueAsync(ExportDataTableInChunks, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "任务处理时发生错误");
            }
        }


        public async Task ExportDataTableInChunks(ExportRequestModel requestmodel)
        {
            _taskServices.UpdateTaskStatus(requestmodel, GlobalTaskStatus.Running, "正在整理数据", 0);

            await _hubContext.SendProgress(requestmodel.UserId, $"正在整理数据");

            int pageSize = 50000; // 每次处理的行数
            int pageIndex = 1; // 分页索引
            bool moreData = true;
            int fileIndex = 0;
            string fileName = string.Empty;
            string msgcontent = string.Empty;
            var Remaining = 0m;

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
                Remaining += pageSize <= resultmodel.Content.recount ? pageSize : resultmodel.Content.recount;
                var percent = Math.Round((Remaining / resultmodel.Content.recount * 100), 2);

                await _hubContext.SendProgress(requestmodel.UserId, $"已导出{percent}%");

                _taskServices.UpdateTaskStatus(requestmodel, GlobalTaskStatus.Running, "已导出", percent);

                pageIndex++;
            }
            msgcontent = $"<p>本次导出为{fileIndex}个文件，请自行点击下载：</p><br/>" + msgcontent;
            //发送消息给自己
            await _messageServices.SendExportMsg("导出文件", msgcontent, _hubContext.GetClaims(requestmodel.UserId));
            await _hubContext.NotifyCompletion(requestmodel.UserId, "导出任务完成");
            _taskServices.UpdateTaskStatus(requestmodel, GlobalTaskStatus.Running, "导出完成", 100);
        }
    }
}
