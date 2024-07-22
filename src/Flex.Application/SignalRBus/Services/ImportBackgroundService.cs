using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.Excel;
using Flex.Core;
using Flex.Core.Config;
using Flex.Core.Extensions.CommonExtensions;
using Flex.Core.Framework.Enum;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.System.ContentModel;
using Flex.Domain.Dtos.System.Upload;
using Flex.SqlSugarFactory.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Collections;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Security.Claims;
using System.Text;

namespace Flex.Application.SignalRBus.Services
{
    public class ImportBackgroundService : BackgroundService
    {
        private readonly IHubNotificationService _hubContext;
        private readonly ILogger<ImportBackgroundService> _logger;
        private readonly IConcurrentQueue<UploadExcelFileDto> _importQueue;
        private IMessageServices _messageServices;
        IColumnContentServices _contentServices;
        IWebHostEnvironment _env;
        string basePath = string.Empty;
        MyContext _sqlsugar;
        ISqlTableServices _sqlTableServices;

        /// <summary>
        /// 文件储存位置
        /// </summary>
        private string exportsFolder = CurrentSiteInfo.SiteUploadPath + $"/Excel/exports/{DateTime.Now.ToDefaultDateTimeStr()}";

        public ImportBackgroundService(
              IHubNotificationService hubContext
            , ILogger<ImportBackgroundService> logger
            , IConcurrentQueue<UploadExcelFileDto> importQueue
            , IWebHostEnvironment env
            , IMessageServices messageServices
            , IColumnContentServices contentServices
            , MyContext myContext
            , ISqlTableServices sqlTableServices
            )
        {
            _hubContext = hubContext;
            _logger = logger;
            _importQueue = importQueue;
            _env = env;
            basePath = _env.WebRootPath + exportsFolder;
            _messageServices = messageServices;
            _contentServices = contentServices;
            _sqlsugar = myContext;
            _sqlTableServices = sqlTableServices;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //这里可以使用队列或其他方式来管理导出请求
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("正在处理导入任务...");
                try
                {
                    var exportRequest = await _importQueue.DequeueAsync(stoppingToken);
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
            var timelist = new List<string>();
            //失败的数据
            var errorlist = new List<Hashtable>();
            var resultmodel = await _contentServices.ImportExcelToModel(uploadExcelFileDto);
            if (!resultmodel.IsSuccess)
            {
                await _hubContext.SendError(uploadExcelFileDto.UserId, resultmodel.Detail);
                await Task.CompletedTask;
                return;
            }
            var dt = resultmodel.Content.dt;
            var orderId = resultmodel.Content.OrderId;
            var statuscode = resultmodel.Content.statuscode;
            var starttime = DateTime.Now;
            var insertSqls = new List<string>();
            var allParameters = new List<SugarParameter>();
            int recordIndex = 1;

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
                InitCreateTable(hashtable, uploadExcelFileDto);
                var insertsql = _sqlTableServices.CreateSqlsugarInsertSqlString(hashtable, resultmodel.Content.TableName, orderId, out parameters, recordIndex);
               

                insertSqls.Add(insertsql.ToString());
                allParameters.AddRange(parameters);

                orderId++;
                recordIndex++;

                
            }
            try
            {
                var result = await _sqlsugar.Db.Ado.ExecuteCommandAsync(string.Join("", insertSqls), allParameters.ToArray());
            }
            catch (Exception ex)
            {
                await _hubContext.SendError(uploadExcelFileDto.UserId, ex.Format());
            }

            //if (errorlist.Count > 0)
            //{
            //    var tablestr = new StringBuilder("<table>");
            //    tablestr.Append("<th>");
            //    foreach (DataColumn item in dt.Columns)
            //    {
            //        tablestr.Append($"<td>{item.Caption}<td>");
            //    }
            //    tablestr.Append("</th>");
            //    tablestr.Append("<tbody>");
            //    foreach (var item in errorlist)
            //    {
            //        tablestr.Append("<tr>");
            //        foreach (var key in item.Keys)
            //        {
            //            tablestr.Append($"<td>{item[key]}<td>");
            //        }
            //        tablestr.Append("</tr>");
            //    }
            //    tablestr.Append("</tbody>");
            //    tablestr.Append("</table>");
            //    await _messageServices.SendExportMsg("导入失败", "以下数据导入失败：<br>" + tablestr, _hubContext.GetClaims(uploadExcelFileDto.UserId));

            //    await _hubContext.SendError(uploadExcelFileDto.UserId, $"有{errorlist.Count}条导入失败的数据");
            //    return;
            //}
            var endtime = DateTime.Now;

            await _messageServices.SendExportMsg("导入成功",$"共导入{dt.Rows.Count}条数据，耗时{(endtime-starttime).TotalSeconds}秒", _hubContext.GetClaims(uploadExcelFileDto.UserId));
            await _hubContext.NotifyCompletion(uploadExcelFileDto.UserId, "导出任务完成");
        }

        private void InitAddTimeAndPublishTime(Hashtable table)
        {
            var addtime = table.GetValue("AddTime")?.ToString() ?? string.Empty;
            var publishtime = table.GetValue("PublishTime")?.ToString() ?? string.Empty;
            //pgSQL情况
            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.PgSql:
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    DateTime utcTime = DateTime.SpecifyKind(Clock.Now, DateTimeKind.Utc);
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                    if (addtime.IsNullOrEmpty())
                        table["AddTime"] = localTime;
                    else
                        table["AddTime"] = addtime.ToUtcTime();
                    if (publishtime.IsNullOrEmpty())
                        table["PublishTime"] = localTime;
                    else
                        table["PublishTime"] = publishtime.ToUtcTime();
                    break;
                default:
                    if (addtime.IsNullOrEmpty())
                        table["AddTime"] = Clock.Now;
                    if (publishtime.IsNullOrEmpty())
                        table["PublishTime"] = Clock.Now;
                    break;
            }
        }
        private void InitCreateTable(Hashtable table, UploadExcelFileDto uploadExcelFileDto)
        {
            var claims = _hubContext.GetClaims(uploadExcelFileDto.UserId);
            table["AddUser"] = uploadExcelFileDto.UserId;
            table["AddUserName"] = claims.UserName;
            table["LastEditUser"] = uploadExcelFileDto.UserId;
            table["LastEditUserName"] = claims.UserName;
            //pgSQL情况
            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.PgSql:
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    DateTime utcTime = DateTime.SpecifyKind(Clock.Now, DateTimeKind.Utc);
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                    table["LastEditDate"] = localTime;
                    break;
                default:
                    table["LastEditDate"] = Clock.Now;
                    break;
            }
            InitAddTimeAndPublishTime(table);
            table["OrderId"] = 0;
        }
    }
}
