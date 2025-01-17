﻿using Castle.Core.Internal;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.ISignalRBus.Model;
using Flex.Core;
using Flex.Core.Extensions.CommonExtensions;
using Flex.Dapper;
using Flex.Dapper.Context;
using Flex.Domain.Dtos.Message;
using Flex.Domain.Dtos.System.Message;
using Flex.Domain.Enums.Message;
using Flex.SqlSugarFactory;
using Flex.SqlSugarFactory.Seed;
using Flex.SqlSugarFactory.UnitOfWorks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mysqlx.Expr;
using System.Collections;
using System.Linq.Expressions;

namespace Flex.Application.Services
{
    public class MessageServices : BaseService, IMessageServices
    {
        private IColumnContentServices contentServices;
        private ISqlTableServices _sqlTableServices;
        private IUnitOfWorkManage _IUnitOfWorkManage;
        private IBaseRepository<sysMessage> _msgrepository;

        public MessageServices(IUnitOfWork unitOfWork,
            IMapper mapper, IdWorker idWorker, IClaimsAccessor claims,
            IColumnContentServices contentServices, ISqlTableServices sqlTableServices,
            IUnitOfWorkManage IUnitOfWorkManage, IBaseRepository<sysMessage> msgrepository)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            this.contentServices = contentServices;
            _sqlTableServices = sqlTableServices;
            _IUnitOfWorkManage = IUnitOfWorkManage;
            _msgrepository = msgrepository;
        }

        private Expression<Func<sysMessage, bool>> GetExpression()
        {
            string userRoleString = _claims.UserRole.ToString();
            string userIdString = _claims.UserId.ToString();
            Expression<Func<sysMessage, bool>> expression =
                    m => ("," + m.ToRoleId + ",").Contains(userRoleString)
                  || ("," + m.ToUserId + ",").Contains(userIdString);

            return expression;
        }
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public async Task<PagedList<MessageTitleListDto>> GetMessageTitleListDtoAsync(int page, int pagesize)
        {

            Func<IQueryable<sysMessage>, IOrderedQueryable<sysMessage>> orderBy =
                m => m.OrderBy(m => m.IsRead).ThenByDescending(m => m.AddTime);

            var list = await _unitOfWork.GetRepository<sysMessage>().GetPagedListAsync(GetExpression(), orderBy, null, page, pagesize);
            PagedList<MessageTitleListDto> model = _mapper
                .Map<PagedList<MessageTitleListDto>>(list);
            return model;
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <returns></returns>
        public int GetNotReadMessageCount()
        {
            var exp = GetExpression();
            exp = exp.And(m => m.IsRead == false);
            var count = _unitOfWork.GetRepository<sysMessage>().Count(exp);
            return count;
        }
        public async Task<ProblemDetails<string>> SendExportMsg(string title, string content, ConnectionModel claims)
        {
            var msg = new sysMessage();
            msg.Title = title;
            msg.AddTime = Clock.Now;
            if (claims != null)
            {
                msg.ToUserId = claims.UserId.ToString();
                msg.AddUser = claims.UserId;
                msg.AddUserName = claims.UserName;
                msg.LastEditUser = claims.UserId;
                msg.LastEditUserName = claims.UserName;
                msg.MsgContent = content;
            }
            try
            {
                var resultmodel = await _unitOfWork.GetRepository<sysMessage>().InsertAsync(msg);
                await _unitOfWork.SaveChangesAsync();
                if (resultmodel.Entity.Id <= 0)
                {
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.MsgSendError.GetEnumDescription());
                }
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.MsgSendSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.MsgSendError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<string>> SendNormalMsg(string title, string content, long ToUserId = 0, long ToRoleId = 0)
        {
            var msg = new sysMessage();
            msg.Title = title;
            msg.AddTime = Clock.Now;
            if (ToUserId != 0)
            {
                msg.ToUserId = ToUserId.ToString();
                msg.AddUser = ToUserId;
                msg.LastEditUser = ToUserId;
            }
            else
            {
                msg.ToUserId = _claims.UserId.ToString();
                msg.AddUser = _claims.UserId;
                msg.AddUserName = _claims.UserName;
                msg.LastEditUser = _claims.UserId;
                msg.LastEditUserName = _claims.UserName;
            }
            if (ToRoleId != 0)
            {
                msg.ToRoleId = ToRoleId.ToString();
            }
            msg.MsgContent = content;
            try
            {
                var resultmodel = await _unitOfWork.GetRepository<sysMessage>().InsertAsync(msg);
                await _unitOfWork.SaveChangesAsync();
                if (resultmodel.Entity.Id <= 0)
                {
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.MsgSendError.GetEnumDescription());
                }
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.MsgSendSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.MsgSendError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<MessageOutputDto>> GetMessageById(int id)
        {
            var exp = GetExpression();
            if (!_claims.IsSystem)
                exp = exp.And(m => m.Id == id);
            else
                exp = m => m.Id == id;
            var model = await _unitOfWork.GetRepository<sysMessage>().GetFirstOrDefaultAsync(exp);
            if (model == null)
                return new ProblemDetails<MessageOutputDto>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            var result = _mapper.Map<MessageOutputDto>(model);
            model.IsRead = true;
            UpdateIntEntityBasicInfo(model);
            _unitOfWork.GetRepository<sysMessage>().Update(model);
            await _unitOfWork.SaveChangesAsync();
            return new ProblemDetails<MessageOutputDto>(HttpStatusCode.OK, result, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId">模型Id</param>
        /// <param name="id">内容Id</param>
        /// <returns></returns>
        public async Task<ProblemDetails<List<ApprovalProcessDto>>> GetApprovalProcessById(int modelId, int id)
        {
            Expression<Func<sysMessage, bool>> exp = m => m.ModelId == modelId && m.ContentId == id;

            var contentrelationlist = (await _unitOfWork.GetRepository<sysMessage>().GetAllAsync(exp, m => m.OrderByDescending(d => d.AddTime))).ToList();
            if ((contentrelationlist?.Count ?? 0) == 0)
                return new ProblemDetails<List<ApprovalProcessDto>>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());

            var firstmodels = contentrelationlist.Where(m => m.IsStart).ToList();
            if ((firstmodels?.Count ?? 0) == 0)
                return new ProblemDetails<List<ApprovalProcessDto>>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            List<ApprovalProcessDto> results = new List<ApprovalProcessDto>();
            foreach (var firstmodel in firstmodels)
            {
                var resultmodel = _mapper.Map<ApprovalProcessDto>(firstmodel);
                contentrelationlist.Remove(firstmodel);
                MapToResult(resultmodel, contentrelationlist);
                results.Add(resultmodel);
            }

            return new ProblemDetails<List<ApprovalProcessDto>>(HttpStatusCode.OK, results);
        }

        private void MapToResult(ApprovalProcessDto currentmodel, List<sysMessage> contentrelationlist)
        {
            var nextmodel = contentrelationlist.FirstOrDefault(m => m.RecieveId == currentmodel.Id && m.MsgGroupId == currentmodel.MsgGroupId);
            if (nextmodel != null)
            {
                var appmodel = _mapper.Map<ApprovalProcessDto>(nextmodel);
                currentmodel.next = appmodel;
                contentrelationlist.Remove(nextmodel);
                MapToResult(currentmodel.next, contentrelationlist);
            }
        }

        private void InitReviewer(Hashtable table)
        {
            table.SetValue("ReviewAddUser", _claims.UserId);
        }
        private void InitContentReviewInfo(Hashtable table, long MsgGroupId, StatusCode statusCode, string ReviewStepId)
        {
            table.SetValue("StatusCode", statusCode.ToInt());
            table.SetValue("ReviewStepId", ReviewStepId);
            table.SetValue("MsgGroupId", MsgGroupId);
        }
        public async Task<ProblemDetails<string>> SendReviewMessage(SendReviewMessageDto model)
        {
            var msgRepository = _unitOfWork.GetRepository<sysMessage>();
            var messagemodel = _mapper.Map<sysMessage>(model);
            var contentmodel = await contentServices.GetSysContentModelByColumnId(model.ParentId);
            if (contentmodel == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());

            if (model.ToPathId.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
            if (model.BaseFormContent.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());

            string updatesql = string.Empty;
            var step = await _unitOfWork.GetRepository<sysWorkFlowStep>().GetFirstOrDefaultAsync(m => m.stepPathId == model.ToPathId);
            if (step == null)
            {
                updatesql = _sqlTableServices.UpdateContentReviewStatus(contentmodel.TableName, model.ContentId, StatusCode.PendingApproval, string.Empty);
                _unitOfWork.ExecuteSqlCommand(updatesql);
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
            }
            messagemodel.Title += $"【{step.stepName}】";

            messagemodel.ToUserId = step.stepMan;
            messagemodel.ToRoleId = step.stepRole;
            messagemodel.FlowId = step.flowId;
            messagemodel.TableName = contentmodel.TableName;

            Func<IQueryable<sysMessage>, IOrderedQueryable<sysMessage>> orderBy = m => m.OrderByDescending(m => m.AddTime);

            var content = await contentServices.GetContentForReviewById(model.ParentId, model.ContentId, model.ModelId);
            if (content == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());

            content.TryGetValue("MsgGroupId", out object gid);
            content.TryGetValue("ReviewAddUser", out object rauer);

            var MsgGroupId = gid?.ToLong() ?? 0;
            var AddUser = rauer?.ToString() ?? string.Empty;

            var endmsg = await msgRepository.GetFirstOrDefaultAsync(m => m.MsgGroupId == MsgGroupId && m.IsEnd, orderBy);
            //判断当前流程是否已结束
            if (endmsg != null)
            {
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewAlreadyComplete.GetEnumDescription());
            }

            var fromstep = await _unitOfWork.GetRepository<sysWorkFlowStep>().GetFirstOrDefaultAsync(m => m.stepPathId == model.FromPathId);
            string result_msg = ErrorCodes.ReviewCreateSuccess.GetEnumDescription();
            if (MsgGroupId == 0 && fromstep.isStart != StepProperty.Start)
            {
                updatesql = _sqlTableServices.UpdateContentReviewStatus(contentmodel.TableName, model.ContentId, StatusCode.PendingApproval, string.Empty);
                _unitOfWork.ExecuteSqlCommand(updatesql);
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewRest.GetEnumDescription());
            }

            if (fromstep.isStart != StepProperty.Start)
            {
                //判断有无当前步骤审核权限
                if (!_claims.IsSystem
                    && !("," + fromstep.stepMan + ",").Contains(_claims.UserId.ToString())
                    && !("," + fromstep.stepRole + ",").Contains(_claims.UserRole.ToString()))
                {
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
                }
                //区分回复的消息
                var parentmsg = await msgRepository.GetFirstOrDefaultAsync(m => m.MsgGroupId == MsgGroupId && m.ToPathId == model.FromPathId, orderBy);
                if (parentmsg != null)
                {
                    messagemodel.RecieveId = parentmsg.Id;
                }
            }

            bool IsStart = false;
            //工作流分组
            if (fromstep.isStart == StepProperty.Start)
            {
                IsStart = true;
                messagemodel.MsgGroupId = _idWorker.NextId();
                messagemodel.MessageCate = MessageCate.Begin;
                messagemodel.IsStart = true;
            }
            else
            {
                result_msg = "发送成功";
                messagemodel.MsgGroupId = MsgGroupId.ToLong();
                messagemodel.IsStart = false;
            }

            //审批通过
            if (step.stepCate == "end")
            {
                messagemodel.IsEnd = true;
                messagemodel.ToUserId = AddUser;
                messagemodel.MessageCate = MessageCate.Approved;
                model.BaseFormContent.SetValue("ReviewAddUser", 0);
                InitContentReviewInfo(model.BaseFormContent, 0, StatusCode.Enable, string.Empty);
            }
            //退稿 设置内容为草稿
            else if (step.stepCate == "end-error")
            {
                messagemodel.IsEnd = true;
                messagemodel.ToUserId = AddUser;
                messagemodel.MessageCate = MessageCate.Rejected;
                model.BaseFormContent.SetValue("ReviewAddUser", 0);
                InitContentReviewInfo(model.BaseFormContent, 0, StatusCode.Draft, string.Empty);
            }
            //否决 设置内容为待审核，并重置流程
            else if (step.stepCate == "end-cancel")
            {
                messagemodel.IsEnd = true;
                messagemodel.ToUserId = AddUser;
                messagemodel.MessageCate = MessageCate.Veto;
                model.BaseFormContent.SetValue("ReviewAddUser", 0);
                InitContentReviewInfo(model.BaseFormContent, 0, StatusCode.PendingApproval, string.Empty);
            }
            else
            {
                InitContentReviewInfo(model.BaseFormContent, messagemodel.MsgGroupId, StatusCode.PendingApproval, model.ToPathId);
                if (fromstep.isStart != StepProperty.Start)
                    messagemodel.MessageCate = MessageCate.NormalTask;
            }

            AddIntEntityBasicInfo(messagemodel);
            _IUnitOfWorkManage.BeginTran();
            try
            {
                var result = new ProblemDetails<int>(0, string.Empty);
                if (model.BaseFormContent != null)
                {
                    if (IsStart)
                    {   //修改提交审批的人，用于判断内容归属
                        InitReviewer(model.BaseFormContent);
                        if (model.BaseFormContent.ContainsKey("Id"))
                            result = await contentServices.Update(model.BaseFormContent, true);
                        else
                            result = await contentServices.Add(model.BaseFormContent, true);
                    }
                    else
                    {
                        result = await contentServices.UpdateReviewContent(model.BaseFormContent, true);
                    }
                }
                if (!result.IsSuccess)
                {
                    _IUnitOfWorkManage.RollbackTran();
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
                }
                if (model.ContentId == 0)
                    messagemodel.ContentId = result.Content;

                var res = await _msgrepository.AddReturnIntAsync(messagemodel);
                if (res <= 0)
                {
                    _IUnitOfWorkManage.RollbackTran();
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
                }
                _IUnitOfWorkManage.CommitTran();
                return Problem<string>(HttpStatusCode.OK, result_msg);
            }
            catch (Exception ex)
            {
                _IUnitOfWorkManage.RollbackTran();
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.ReviewCreateError.GetEnumDescription(), ex);
            }
        }
    }
}
