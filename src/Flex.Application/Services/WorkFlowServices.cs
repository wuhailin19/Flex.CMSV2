using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.WorkFlow;
using Flex.EFSqlServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShardingCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class WorkFlowServices : BaseService, IWorkFlowServices
    {
        public WorkFlowServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
        /// <summary>
        /// 获取工作流列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<PagedList<WorkFlowColumnDto>> GetWorkFlowListAsync(int page, int pagesize)
        {
            var list = await _unitOfWork.GetRepository<sysWorkFlow>().GetPagedListAsync(null, null, null, page, pagesize);
            PagedList<WorkFlowColumnDto> workflows = _mapper.Map<PagedList<WorkFlowColumnDto>>(list);
            return workflows;
        }
        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<sysWorkFlow>();
            if (Id.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<sysWorkFlow>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                }
                adminRepository.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<ProblemDetails<string>> Add(InputWorkFlowAddDto inputWorkFlowContentDto)
        {
            var workflowresponsity = _unitOfWork.GetRepository<sysWorkFlow>();
            var model = _mapper.Map<sysWorkFlow>(inputWorkFlowContentDto);
            AddIntEntityBasicInfo(model);
            try
            {
                workflowresponsity.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
        }
        
        /// <summary>
        /// 修改流程图
        /// </summary>
        /// <returns></returns>
        public async Task<ProblemDetails<string>> UpdateFlowChat(InputWorkFlowContentDto inputWorkFlowContentDto)
        {
            var model = new WorkFlowDto
            {
                Id = inputWorkFlowContentDto.Id,
                WorkFlowContent = DecodeData(inputWorkFlowContentDto.design),
                actDesign = DecodeData(inputWorkFlowContentDto.actDesign),
                stepDesign = DecodeData(inputWorkFlowContentDto.stepDesign)
            };

            WorkflowData workflowData = JsonConvert.DeserializeObject<WorkflowData>(model.WorkFlowContent);
            var step = DeserializeDesign<List<Dictionary<string, StepObject>>>(model.stepDesign);
            var action = DeserializeDesign<List<Dictionary<string, ActionObject>>>(model.actDesign);

            var states = workflowData.states;
            var paths = workflowData.paths;

            var step_list = CreateWorkFlowSteps(states, step, model.Id);
            var action_list = CreateWorkFlowActions(paths, action, states, model.Id, out string actionstr);
            model.actionString = actionstr;

            var workflowresponsity = _unitOfWork.GetRepository<sysWorkFlow>();
            var workflow = await workflowresponsity.GetFirstOrDefaultAsync(m => m.Id == model.Id);
            var baseactionRepository = _unitOfWork.GetRepository<sysWorkFlowAction>();
            var basestepRepository = _unitOfWork.GetRepository<sysWorkFlowStep>();

            _unitOfWork.SetTransaction();
            try
            {
                await DeleteExistingEntities(model.Id);
                if (action_list.Count > 0)
                    await baseactionRepository.InsertAsync(action_list);

                if (step_list.Count > 0)
                    await basestepRepository.InsertAsync(step_list);
                workflowresponsity.Update(_mapper.Map(model, workflow));

                await _unitOfWork.SaveChangesTranAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
        }
        private async Task<bool> DeleteExistingEntities(int flowId)
        {
            var workflowstrp = _unitOfWork.GetRepository<sysWorkFlowStep>();
            var workflowaction = _unitOfWork.GetRepository<sysWorkFlowAction>();
            try
            {
                var steps = await workflowstrp.GetAllAsync(m => m.flowId == flowId);
                workflowstrp.Delete(steps);
                
                var actions = await workflowaction.GetAllAsync(m => m.flowId == flowId);
                workflowaction.Delete(actions);
                return true;
            }
            catch
            {
                throw;
            }
        }
        private List<sysWorkFlowStep> CreateWorkFlowSteps(Dictionary<string, State> states, List<Dictionary<string, StepObject>> step, int flowId)
        {
            var step_list = new List<sysWorkFlowStep>();

            foreach (var flowaction in states.Keys)
            {
                var currentpath = states[flowaction];
                var actionstep = GetDictionaryValueOrDefault(step, flowaction) ?? new StepObject();

                step_list.Add(new sysWorkFlowStep
                {
                    avoidFlag = actionstep.AvoidFlag,
                    stepOrg = actionstep.StepOrg,
                    stepRole = actionstep.StepRole,
                    stepPathId = flowaction,
                    stepName = currentpath.text.text,
                    flowId = flowId,
                    orgMode = actionstep.OrgMode,
                    stepMan = actionstep.StepMan,
                    Id = _idWorker.NextId().ToString(),
                    isStart = currentpath.type == "start" ? StepProperty.Start : StepProperty.Other
                });
            }

            return step_list;
        }
        private List<sysWorkFlowAction> CreateWorkFlowActions(Dictionary<string, PathObject> paths, List<Dictionary<string, ActionObject>> action, Dictionary<string, State> states, int flowId, out string actionstr)
        {
            var action_list = new List<sysWorkFlowAction>();
            actionstr = string.Empty;

            foreach (var flowstep in paths.Keys)
            {
                var currentpath = paths[flowstep];
                var actionstep = GetDictionaryValueOrDefault(action, flowstep) ?? new ActionObject();

                var flowAction = new sysWorkFlowAction
                {
                    Id = _idWorker.NextId().ToString(),
                    actionPathId = flowstep,
                    flowId = flowId,
                    stepFromId = currentpath.from,
                    stepToId = currentpath.to,
                    actionName = currentpath.text.text,
                    conjunctManFlag = actionstep.ConjunctManFlag,
                    directMode = actionstep.DirectMode,
                    orgBossMode = actionstep.OrgBossMode
                };
                flowAction.actionFromName = states[flowAction.stepFromId].text.text;
                flowAction.actionToName = states[flowAction.stepToId].text.text;

                if (actionstr.IsEmpty())
                    actionstr += flowAction.actionName;
                else
                    actionstr += "," + flowAction.actionName;

                action_list.Add(flowAction);
            }

            return action_list;
        }
        private T DeserializeDesign<T>(string design)
        {
            return JsonConvert.DeserializeObject<T>(design);
        }
        private T GetDictionaryValueOrDefault<T>(List<Dictionary<string, T>> list, string key)
        {
            return list
                .SelectMany(dict => dict)
                .Where(kvp => kvp.Key == key)
                .Select(kvp => kvp.Value)
                .FirstOrDefault();
        }

        public async Task<InputEditStepManagerDto> GetStepManagerById(string stepId)
        {
            var stepresponsity = _unitOfWork.GetRepository<sysWorkFlowStep>();
            return _mapper.Map<InputEditStepManagerDto>(
                await stepresponsity.GetFirstOrDefaultAsync(m => m.stepPathId == stepId));
        }
        public async Task<ProblemDetails<string>> UpdateStepManager(InputEditStepManagerDto inputEditStepManagerDto)
        {
            var workflowresponsity = _unitOfWork.GetRepository<sysWorkFlowStep>();
            var model = await workflowresponsity.GetFirstOrDefaultAsync(m => m.stepPathId == inputEditStepManagerDto.Id);
            UpdateStringEntityBasicInfo(model);
            model.stepRole = inputEditStepManagerDto.stepRole;
            model.stepMan = inputEditStepManagerDto.stepMan;
            try
            {
                workflowresponsity.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
        }
    }
}
