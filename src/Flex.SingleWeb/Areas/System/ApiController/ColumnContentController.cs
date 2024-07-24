using Consul;
using Flex.Application.Authorize;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.ISignalRBus.Enum;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Core.Attributes;
using Flex.Core.Helper;
using Flex.Core.Timing;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Flex.Domain.Dtos.System.ColumnContent;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Flex.WebApi.SystemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "栏目内容相关接口")]
    public class ColumnContentController : ApiBaseController
    {
        private IColumnContentServices _columnServices;
        IClaimsAccessor _claims;
        private ITaskServices _taskServices;
        private IdWorker _idWorker;
        public ColumnContentController(
            IColumnContentServices columnServices
            , IClaimsAccessor claims
            , ITaskServices taskServices
            , IdWorker idWorker
            )
        {
            _columnServices = columnServices;
            _claims = claims;
            _taskServices = taskServices;
            _idWorker = idWorker;
        }

        #region 表头信息
        [HttpGet("Column/{modelId}/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> Column(int modelId, int ParentId)
        {
            return Success(await _columnServices.GetTableThs(ParentId, modelId));
        }

        [HttpGet("HistoryColumn/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> HistoryColumn(int ParentId)
        {
            return Success(await _columnServices.GetHistoryTableThs(ParentId));
        }

        [HttpGet("SoftDeleteColumn/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> SoftDeleteColumn(int ParentId)
        {
            return Success(await _columnServices.GetSoftTableThs(ParentId));
        }
        #endregion

        #region 查询

        /// <summary>
        /// 多选checkbox数据用此接口
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        [HttpGet("ContentOptions/{ParentId}")]
        [Descriper(IsFilter = true)]
        public async Task<string> GetContentOptions(int ParentId)
        {
            return Success(await _columnServices.GetContentOptions(ParentId));
        }

        [HttpGet("ListAsync")]
        [Descriper(Name = "栏目内容列表分页数据")]
        public async Task<string> ListAsync([FromQuery] ContentPageListParamDto model)
        {
            if (model == null)
                return Fail("无数据");
            return Success(await _columnServices.ListAsync(model));
        }

        [HttpGet("HistoryListAsync")]
        [Descriper(Name = "历史内容列表分页数据")]
        public async Task<string> HistoryListAsync([FromQuery] ContentPageListParamDto model)
        {
            if (model == null)
                return Fail("无数据");
            return Success(await _columnServices.HistoryListAsync(model));
        }

        [HttpGet("SoftDeleteListAsync")]
        [Descriper(Name = "回收站列表分页数据")]
        public async Task<string> SoftDeleteListAsync([FromQuery] ContentPageListParamDto model)
        {
            if (model == null)
                return Fail("无数据");
            return Success(await _columnServices.SoftDeleteListAsync(model));
        }

        [HttpGet("GetFormHtml/{currentmodelId}")]
        [Descriper(Name = "通过ParentId获取模型结构代码")]
        public async Task<string> GetFormHtml(int currentmodelId)
        {
            var result = await _columnServices.GetFormHtml(currentmodelId);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpGet("GetContentById/{modelId}/{ParentId}/{Id}")]
        [Descriper(Name = "通过modelId、ParentId和Id获取栏目内容", IsFilter = true)]
        public async Task<string> GetContentById(int modelId, int ParentId, int Id)
        {
            if (Id == 0)
                return Success(await _columnServices.GetButtonListByParentId(ParentId));
            var result = await _columnServices.GetContentById(modelId, ParentId, Id);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Content);
        }
        #endregion

        #region 新增
        [HttpPost("CreateColumnContent")]
        [Descriper(Name = "新增栏目内容")]
        public async Task<string> Add()
        {
            var model = await GetModel<Hashtable>();
            var result = await _columnServices.Add(model);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        #endregion

        #region 修改
        [HttpPost]
        [Descriper(Name = "修改栏目内容")]
        public async Task<string> Update()
        {
            var model = await GetModel<Hashtable>();
            var result = await _columnServices.Update(model);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("UpdateStatus")]
        [Descriper(Name = "修改栏目内容状态")]
        public async Task<string> UpdateStatus()
        {
            var model = await GetModel<Hashtable>();
            var result = await _columnServices.SimpleUpdateContent(model);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("CancelReview")]
        [Descriper(Name = "取消审批")]
        public async Task<string> CancelReview()
        {
            var model = await GetModel<Hashtable>();
            model["ReviewAddUser"] = string.Empty;
            model["MsgGroupId"] = string.Empty;
            var result = await _columnServices.UpdateReviewContent(model, true, true);
            if (!result.IsSuccess)
                return Fail("审批取消失败");
            return Success(result.Detail);
        }
        #endregion

        #region 删除相关
        [HttpPost("CompletelyDelete/{modelId}/{parentId}/{Id}")]
        [Descriper(Name = "完全删除栏目内容，不可恢复")]
        public async Task<string> CompletelyDelete(int modelId, int parentId, string Id)
        {
            var result = await _columnServices.CompletelyDelete(parentId, modelId, Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("RestContent/{modelId}/{parentId}/{Id}")]
        [Descriper(Name = "从回收站恢复栏目内容")]
        public async Task<string> RestContent(int modelId, int parentId, string Id)
        {
            var result = await _columnServices.RestContent(parentId, modelId, Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("{modelId}/{parentId}/{Id}")]
        [Descriper(Name = "软删除栏目内容，可恢复")]
        public async Task<string> Delete(int modelId, int parentId, string Id)
        {
            var result = await _columnServices.Delete(parentId, modelId, Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        #endregion

        #region 数据操作
        [HttpPost("ContentTools")]
        [Descriper(Name = "数据复制移动和引用")]
        public async Task<string> ContentTools()
        {
            var validatemodel = await ValidateModel<ContentToolsDto>();
            if (!validatemodel.IsSuccess)
                return Fail(validatemodel.Detail);
            var result = await _columnServices.ContentOperation(validatemodel.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        #endregion

        #region 导入导出
        [HttpGet("ExportExcel")]
        [Descriper(Name = "导出Excel")]
        public async Task<string> ExportExcel([FromQuery] ExportRequestModel requestmodel)
        {
            requestmodel.UserId = _claims.UserId;
            requestmodel.CurrrentTaskId = _idWorker.NextId();


            await _taskServices.AddTaskAsync(requestmodel.UserId, new TaskModel<RequestModel>
            {
                TaskId = requestmodel.CurrrentTaskId,
                Name = "导出Excel",
                AddTime = Clock.Now,
                Status = GlobalTaskStatus.Waiting,
                StatusString = GlobalTaskStatus.Waiting.GetEnumDescription(),
                TaskCate = requestmodel
            });
            //await _exportQueue.EnqueueAsync(requestmodel); // 将请求加入队列

            return Success("导出任务已生成");

            #region 起初用文件流直接返回的，数据量大的时候太慢了，改造成Signalr异步通知结果了
            //var stream = ContentModelHelper.SimpleExportToSpreadsheet(resultmodel.Content.result, resultmodel.Content.filedModels);
            //var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //var fileName = resultmodel.Content.ExcelName + ".xlsx";
            //// 返回文件流作为文件下载
            //return File(stream, contentType, fileName);
            #endregion
        }

        [HttpPost("ImportExcel")]
        [Descriper(Name = "导入Excel")]
        public async Task<string> ImportExcel([FromForm] ImportRequestModel uploadExcelFileDto)
        {

            if (uploadExcelFileDto.file == null)
                return Fail(ErrorCodes.UploadTypeDenied.GetEnumDescription());
            var file = uploadExcelFileDto.file[0];
            if (!FileCheckHelper.IsAllowedExcelExtension(file))
                return Fail(ErrorCodes.UploadTypeDenied.GetEnumDescription());
            uploadExcelFileDto.UserId = _claims.UserId;

            using (var originalStream = uploadExcelFileDto.file[0].OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    originalStream.CopyTo(memoryStream);
                    memoryStream.Position = 0;

                    // 将内存流保存到 uploadExcelFileDto，以便在队列中使用
                    uploadExcelFileDto.FileContent = memoryStream.ToArray();
                }
            }
            uploadExcelFileDto.CurrrentTaskId = _idWorker.NextId();
            await _taskServices.AddTaskAsync(uploadExcelFileDto.UserId, new TaskModel<RequestModel>
            {
                TaskId = uploadExcelFileDto.CurrrentTaskId,
                Name = "导入Excel",
                AddTime = Clock.Now,
                Status = GlobalTaskStatus.Waiting,
                StatusString = GlobalTaskStatus.Waiting.GetEnumDescription(),
                TaskCate = uploadExcelFileDto
            });

            //await _importQueue.EnqueueAsync(uploadExcelFileDto);

            return Success("导入任务已生成");
        }
        #endregion
    }
}
