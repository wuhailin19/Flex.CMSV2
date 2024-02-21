using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.Message;
using Flex.Domain.Enums.Message;
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
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<PagedList<MessageTitleListDto>> GetMessageTitleListDtoAsync(int page, int pagesize)
        {
            string userRoleString = _claims.UserRole.ToString();
            string userIdString = _claims.UserId.ToString();

            Expression<Func<sysMessage, bool>> expression =
                m => ("," + m.ToRoleId + ",").Contains(userRoleString)
              || ("," + m.ToUserId + ",").Contains(userIdString);

            Func<IQueryable<sysMessage>, IOrderedQueryable<sysMessage>> orderBy =
                m => m.OrderBy(m => m.IsRead).ThenByDescending(m => m.AddTime);

            var list = await _unitOfWork.GetRepository<sysMessage>().GetPagedListAsync(expression, orderBy, null, page, pagesize);
            PagedList<MessageTitleListDto> model = _mapper
                .Map<PagedList<MessageTitleListDto>>(list);
            return model;
        }

        public async Task<MessageOutputDto> GetMessageById(int id)
        {
            var model = await _unitOfWork.GetRepository<sysMessage>().GetFirstOrDefaultAsync(m => m.Id == id);
            var result = _mapper.Map<MessageOutputDto>(model);
            model.IsRead = true;
            UpdateIntEntityBasicInfo(model);
            _unitOfWork.GetRepository<sysMessage>().Update(model);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }


        public async Task<ProblemDetails<string>> SendReviewMessage(SendReviewMessageDto model)
        {
            var msgRepository = _unitOfWork.GetRepository<sysMessage>();
            var messagemodel = _mapper.Map<sysMessage>(model);

            if (model.ToPathId.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());

            var step = await _unitOfWork.GetRepository<sysWorkFlowStep>().GetFirstOrDefaultAsync(m => m.stepPathId == model.ToPathId);
            if (step == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
            messagemodel.ToUserId = step.stepMan;
            messagemodel.ToRoleId = step.stepRole;

            var contentmodel = await contentServices.GetSysContentModelByColumnId(model.ParentId);
            if (contentmodel == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
            string updatesql = string.Empty;
            messagemodel.TableName = contentmodel.TableName;
            var firstmsg = await msgRepository.GetFirstOrDefaultAsync(m => m.ContentId == model.ContentId && m.ParentId == model.ParentId && m.IsStart);

            //审批通过
            if (step.stepCate == "end")
            {
                messagemodel.ToUserId = firstmsg.AddUser.ToString();
                messagemodel.MessageCate = MessageCate.Approved;
                updatesql = _sqlTableServices.UpdateContentReviewStatus(contentmodel.TableName, model.ContentId, StatusCode.Enable, string.Empty);
            }
            //审批驳回或退稿
            else if (step.stepCate == "end-error" || step.stepCate == "end-cancel")
            {
                messagemodel.ToUserId = firstmsg.AddUser.ToString();
                messagemodel.MessageCate = MessageCate.Rejected;
                updatesql = _sqlTableServices.UpdateContentReviewStatus(contentmodel.TableName, model.ContentId, StatusCode.Draft, string.Empty);
            }
            else
            {
                updatesql = _sqlTableServices.UpdateContentReviewStatus(contentmodel.TableName, model.ContentId, StatusCode.PendingApproval, model.ToPathId);
                messagemodel.MessageCate = MessageCate.NormalTask;
            }
            var fromstep = await _unitOfWork.GetRepository<sysWorkFlowStep>().GetFirstOrDefaultAsync(m => m.stepPathId == model.FromPathId);
            //工作流分组
            if (fromstep.isStart == StepProperty.Start)
            {
                messagemodel.MsgGroupId = _idWorker.NextId();
                messagemodel.IsStart = true;
            }
            else
            {
                messagemodel.MsgGroupId = firstmsg.MsgGroupId;
                messagemodel.IsStart = false;
            }
            AddIntEntityBasicInfo(messagemodel);
            _unitOfWork.SetTransaction();
            try
            {
                if (updatesql.IsNotNullOrEmpty())
                    _unitOfWork.ExecuteSqlCommand(updatesql);
                msgRepository.Insert(messagemodel);
                await _unitOfWork.SaveChangesTranAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.ReviewCreateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.ReviewCreateError.GetEnumDescription());
            }
        }
    }
}
