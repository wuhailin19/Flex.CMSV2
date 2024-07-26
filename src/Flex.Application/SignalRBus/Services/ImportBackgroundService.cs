using Flex.Application.Contracts.ISignalRBus.Enum;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Core.Config;
using Flex.Core.Extensions.CommonExtensions;
using Flex.Core.Framework.Enum;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Flex.SqlSugarFactory.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Collections;
using System.Data;

namespace Flex.Application.SignalRBus.Services
{
    public class ImportBackgroundService : BackgroundService
    {
        private readonly IHubNotificationService _hubContext;
        private readonly ILogger<ImportBackgroundService> _logger;
        private readonly IConcurrentQueue<ImportRequestModel> _importQueue;
        private IMessageServices _messageServices;
        IColumnContentServices _contentServices;
        IWebHostEnvironment _env;
        string basePath = string.Empty;
        MyContext _sqlsugar;
        ISqlTableServices _sqlTableServices;
        private ITaskServices _taskServices;

        /// <summary>
        /// 文件储存位置
        /// </summary>
        private string exportsFolder = CurrentSiteInfo.SiteUploadPath + $"/Excel/exports/{DateTime.Now.ToDefaultDateTimeStr()}";

        public ImportBackgroundService(
              IHubNotificationService hubContext
            , ILogger<ImportBackgroundService> logger
            , IConcurrentQueue<ImportRequestModel> importQueue
            , IWebHostEnvironment env
            , IMessageServices messageServices
            , IColumnContentServices contentServices
            , MyContext myContext
            , ITaskServices taskServices
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
            _taskServices = taskServices;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("正在处理导入任务...");
            try
            {
                await _importQueue.ProcessQueueAsync(ImportDataTableInChunks, stoppingToken);
            }
            catch (TaskHandledException ex)
            {
                _logger.LogError(ex.Format(), "任务处理时发生错误");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Format(), "任务处理时发生错误");
            }
        }

        public async Task ImportDataTableInChunks(ImportRequestModel uploadExcelFileDto)
        {
            _taskServices.UpdateTaskStatus(uploadExcelFileDto, GlobalTaskStatus.Start, "正在准备导入", 0);

            await _hubContext.SendProgress(uploadExcelFileDto.UserId, $"正在准备导入");

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
            var allcount = dt.Rows.Count;
            var Remaining = 0m;
            var pageSize = 2000;//单次导入条数

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
                hashtable.Add("PId", uploadExcelFileDto.PId);
                InitCreateTable(hashtable, uploadExcelFileDto);
                var insertsql = _sqlTableServices.CreateSqlsugarInsertSqlString(hashtable, resultmodel.Content.TableName, orderId, out parameters, recordIndex);

                insertSqls.Add(insertsql.ToString());
                allParameters.AddRange(parameters);

                orderId++;
                recordIndex++;

                //两千条数据提交一次
                if (recordIndex % 2000 == 0)
                {
                    await _sqlsugar.Db.Ado.ExecuteCommandAsync(string.Join("", insertSqls), allParameters.ToArray());
                    recordIndex = 1;
                    insertSqls = new List<string>();
                    hashtable = new Hashtable();
                    allParameters = new List<SugarParameter>();

                    Remaining += pageSize;
                    Remaining = Remaining <= allcount ? Remaining : allcount;
                    var percent = Math.Round((Remaining / allcount * 100), 2);

                    await _hubContext.SendProgress(uploadExcelFileDto.UserId, $"已导入{percent}%");

                    _taskServices.UpdateTaskStatus(uploadExcelFileDto, GlobalTaskStatus.Running, "正在导入", percent);
                }
            }
            try
            {
                await _sqlsugar.Db.Ado.ExecuteCommandAsync(string.Join("", insertSqls), allParameters.ToArray());
            }
            catch (Exception ex)
            {
                await _hubContext.SendError(uploadExcelFileDto.UserId, ex.Format());
            }

            var endtime = DateTime.Now;

            await _messageServices.SendExportMsg("导入成功", $"共导入{allcount}条数据，耗时{(endtime - starttime).TotalSeconds}秒", _hubContext.GetClaims(uploadExcelFileDto.UserId));
            await _hubContext.NotifyCompletion(uploadExcelFileDto.UserId, "导入任务完成");

            _taskServices.UpdateTaskStatus(uploadExcelFileDto, GlobalTaskStatus.Ending, "导入完成", 100);
        }
        #region 弃用
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
        #endregion
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
        private void InitCreateTable(Hashtable table, ImportRequestModel uploadExcelFileDto)
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
