using Flex.Domain.Dtos.Message;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/{controller}")]
    public class MessageController : ApiBaseController
    {
        IMessageServices _messageServices;
        public MessageController(IMessageServices messageServices)
        {
            _messageServices = messageServices;
        }

        [HttpGet("GetMessageTitleListDtoAsync")]
        public async Task<string> GetMessageTitleListDtoAsync(int page)
        {
            return Success(await _messageServices.GetMessageTitleListDtoAsync(page, 10));
        }
        [HttpGet("GetMessage/{id}")]
        public async Task<string> GetMessageById(int id)
        {
            return Success(await _messageServices.GetMessageById(id));
        }

        [HttpPost("SendReviewMessage")]
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
