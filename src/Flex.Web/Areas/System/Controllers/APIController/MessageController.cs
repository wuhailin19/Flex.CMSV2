using Flex.Core.Attributes;
using Flex.Domain.Dtos.Message;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/{controller}")]
    [Descriper(Name = "消息相关接口")]
    public class MessageController : ApiBaseController
    {
        IMessageServices _messageServices;
        public MessageController(IMessageServices messageServices)
        {
            _messageServices = messageServices;
        }

        [HttpGet("GetMessageTitleListDtoAsync")]
        [Descriper(Name = "获取消息列表")]
        public async Task<string> GetMessageTitleListDtoAsync(int page)
        {
            return Success(await _messageServices.GetMessageTitleListDtoAsync(page, 10));
        }
        [HttpGet("GetMessage/{id}")]
        [Descriper(Name = "获取信息内容")]
        public async Task<string> GetMessageById(int id)
        {
            return Success(await _messageServices.GetMessageById(id));
        }

        [HttpPost("SendReviewMessage")]
        [Descriper(Name = "发送审批信息")]
        public async Task<string> SendReviewMessage()
        {
            var model = await GetModel<SendReviewMessageDto>();
            var result = await _messageServices.SendReviewMessage(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
