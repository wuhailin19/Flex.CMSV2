using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.WorkFlow;
using Flex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IWorkFlowServices
    {
        Task<ProblemDetails<string>> Add(InputWorkFlowAddDto inputWorkFlowContentDto);
        Task<ProblemDetails<string>> Delete(string Id);
        Task<IEnumerable<StepActionButtonDto>> GetStepActionButtonList(InputWorkFlowStepDto stepDto);
        Task<InputEditStepManagerDto> GetStepManagerById(string stepId);
        Task<PagedList<WorkFlowColumnDto>> GetWorkFlowListAsync(int page, int pagesize);
        Task<IEnumerable<WorkFlowSelectDto>> GetWorkFlowSelectDtoListAsync();
        Task<ProblemDetails<string>> Update(InputWorkFlowUpdateDto updatedto);
        Task<ProblemDetails<string>> UpdateFlowChat(InputWorkFlowContentDto inputWorkFlowContentDto);
        Task<ProblemDetails<string>> UpdateStepManager(InputEditStepManagerDto inputEditStepManagerDto);
    }
}
