using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.Message;
using Flex.Domain.Enums.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class MessageServices : BaseService, IMessageServices
    {
        public MessageServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
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

            Func<IQueryable<sysMessage>, IOrderedQueryable<sysMessage>> orderBy = m => m.OrderBy(m => m.IsRead).ThenByDescending(m=>m.AddTime);

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
            var menuRepository = _unitOfWork.GetRepository<sysMessage>();
            var messagemodel = _mapper.Map<sysMessage>(model);
            if (model.stepId.IsNotNullOrEmpty())
            {
                var step = await _unitOfWork.GetRepository<sysWorkFlowStep>().GetFirstOrDefaultAsync(m => m.stepPathId == model.stepId);
                if (step != null)
                {
                    messagemodel.ToUserId = step.stepMan;
                    messagemodel.ToRoleId = step.stepRole;
                    messagemodel.MessageCate = MessageCate.Review;
                }
            }
            AddIntEntityBasicInfo(messagemodel);
            try
            {
                menuRepository.Insert(messagemodel);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
        }
    }
}
