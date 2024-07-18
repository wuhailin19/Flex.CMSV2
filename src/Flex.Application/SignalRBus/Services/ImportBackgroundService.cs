using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.Excel;
using Flex.Core.Config;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.System.ContentModel;
using Flex.Domain.Dtos.System.Upload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Flex.Application.SignalRBus.Services
{
    public class ImportBackgroundService : BackgroundService
    {
        private readonly IHubNotificationService _hubContext;
        private readonly ILogger<ImportBackgroundService> _logger;
        private readonly IConcurrentQueue<UploadExcelFileDto> _exportQueue;
        private IMessageServices _messageServices;
        IColumnContentServices _contentServices;
        IWebHostEnvironment _env;
        string basePath = string.Empty;

        /// <summary>
        /// 文件储存位置
        /// </summary>
        private string exportsFolder = CurrentSiteInfo.SiteUploadPath + $"/Excel/exports/{DateTime.Now.ToDefaultDateTimeStr()}";

        public ImportBackgroundService(
              IHubNotificationService hubContext
            , ILogger<ImportBackgroundService> logger
            , IConcurrentQueue<UploadExcelFileDto> exportQueue
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
                _logger.LogInformation("正在处理导入任务...");
                try
                {
                    var exportRequest = await _exportQueue.DequeueAsync(stoppingToken);
                    if (exportRequest != null)
                    {
                        //_logger.LogInformation("开始导入");
                        await ImportDataTableInChunks(exportRequest);
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
        public async Task ImportDataTableInChunks(UploadExcelFileDto uploadExcelFileDto)
        {
            

        }
    }
}
