using Castle.Core.Internal;
using Flex.Application.Contracts.Exceptions;
using Flex.Core;
using Flex.Core.Extensions.CommonExtensions;
using Flex.Domain.Dtos.Message;
using Flex.Domain.Enums.Message;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Linq.Expressions;

namespace Flex.Application.Services
{
    public class MessageServices : BaseService, IMessageServices
    {
        IColumnContentServices contentServices;
        ISqlTableServices _sqlTableServices;

        public MessageServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, IColumnContentServices contentServices, ISqlTableServices sqlTableServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            this.contentServices = contentServices;
            _sqlTableServices = sqlTableServices;
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
        /// <param name="limit"></param>
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
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public int GetNotReadMessageCount()
        {
            var exp = GetExpression();
            exp = exp.And(m => m.IsRead == false);
            var count = _unitOfWork.GetRepository<sysMessage>().Count(exp);
            return count;
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
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());

            if (model.ToPathId.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
            if (model.BaseFormContent.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());

            string updatesql = string.Empty;
            var step = await _unitOfWork.GetRepository<sysWorkFlowStep>().GetFirstOrDefaultAsync(m => m.stepPathId == model.ToPathId);
            if (step == null)
            {
                updatesql = _sqlTableServices.UpdateContentReviewStatus(contentmodel.TableName, model.ContentId, StatusCode.PendingApproval, string.Empty);
                _unitOfWork.ExecuteSqlCommand(updatesql);
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
            }

            
            messagemodel.ToUserId = step.stepMan;
            messagemodel.ToRoleId = step.stepRole;
            messagemodel.FlowId = step.flowId;
            messagemodel.TableName = contentmodel.TableName;

            Func<IQueryable<sysMessage>, IOrderedQueryable<sysMessage>> orderBy = m => m.OrderByDescending(m => m.AddTime);

            var content = await contentServices.GetContentForReviewById(model.ParentId, model.ContentId);
            if (content == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());

            content.TryGetValue("MsgGroupId", out object gid);
            content.TryGetValue("ReviewAddUser", out object rauer);

            var MsgGroupId = gid?.ToLong() ?? 0;
            var AddUser = rauer?.ToString() ?? string.Empty;

            var endmsg = await msgRepository.GetFirstOrDefaultAsync(m => m.MsgGroupId == MsgGroupId && m.IsEnd, orderBy);
            //判断当前流程是否已结束
            if (endmsg != null)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewAlreadyComplete.GetEnumDescription());
            }

            var fromstep = await _unitOfWork.GetRepository<sysWorkFlowStep>().GetFirstOrDefaultAsync(m => m.stepPathId == model.FromPathId);
            string result_msg = ErrorCodes.ReviewCreateSuccess.GetEnumDescription();
            if (MsgGroupId == 0 && fromstep.isStart != StepProperty.Start)
            {
                //判断有无当前步骤审核权限
                if (!_claims.IsSystem
                    && !("," + fromstep.stepMan + ",").Contains(_claims.UserId.ToString())
                    && !("," + fromstep.stepRole + ",").Contains(_claims.UserRole.ToString()))
                {
                    return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
                }

                updatesql = _sqlTableServices.UpdateContentReviewStatus(contentmodel.TableName, model.ContentId, StatusCode.PendingApproval, string.Empty);
                _unitOfWork.ExecuteSqlCommand(updatesql);
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewRest.GetEnumDescription());
            }

            bool IsStart = false;
            //工作流分组
            if (fromstep.isStart == StepProperty.Start)
            {
                IsStart = true;
                messagemodel.MsgGroupId = _idWorker.NextId();
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
            //审批驳回 设置内容为草稿
            else if (step.stepCate == "end-error")
            {
                messagemodel.IsEnd = true;
                messagemodel.ToUserId = AddUser;
                messagemodel.MessageCate = MessageCate.Rejected;
                model.BaseFormContent.SetValue("ReviewAddUser", 0);
                InitContentReviewInfo(model.BaseFormContent, 0, StatusCode.Draft, string.Empty);
            }
            //退稿 设置内容为待审核，并重置流程
            else if (step.stepCate == "end-cancel")
            {
                messagemodel.IsEnd = true;
                messagemodel.ToUserId = AddUser;
                messagemodel.MessageCate = MessageCate.Rejected;
                model.BaseFormContent.SetValue("ReviewAddUser", 0);
                InitContentReviewInfo(model.BaseFormContent, 0, StatusCode.PendingApproval, string.Empty);
            }
            else
            {
                InitContentReviewInfo(model.BaseFormContent, messagemodel.MsgGroupId, StatusCode.PendingApproval, model.ToPathId);
                messagemodel.MessageCate = MessageCate.NormalTask;
            }

            AddIntEntityBasicInfo(messagemodel);
            _unitOfWork.SetTransaction();
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
                    await _unitOfWork.RollbackAsync();
                    return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
                }
                if (model.ContentId == 0)
                    messagemodel.ContentId = result.Content;

                msgRepository.Insert(messagemodel);
                await _unitOfWork.SaveChangesTranAsync();

                return new ProblemDetails<string>(HttpStatusCode.OK, result_msg);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
            }
        }
    }
}
