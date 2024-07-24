using Flex.Application.Authorize;
using Flex.Application.Contracts.ISignalRBus.IServices;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ApiController
{
    [Route("api/[controller]")]
    public class TaskController : ApiBaseController
    {
        private ITaskServices _taskServices;
        IClaimsAccessor _claims;
        public TaskController(
            ITaskServices taskServices,
            IClaimsAccessor claims
            )
        {
            _taskServices = taskServices;
            _claims = claims;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllTaskList")]
        public string GetAllTaskList()
        {
            var result = _taskServices.GetTaskModelsAsync(_claims.UserId);
            return Success(result, 200);
        }
    }
}
