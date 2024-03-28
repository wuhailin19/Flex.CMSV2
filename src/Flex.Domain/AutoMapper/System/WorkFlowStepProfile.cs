using Flex.Domain.Collections;
using Flex.Domain.Dtos.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.AutoMapper
{
    public class WorkFlowStepProfile : Profile
    {
        public WorkFlowStepProfile()
        {
            CreateMap<sysWorkFlowStep, InputEditStepManagerDto>();
        }
    }
}
