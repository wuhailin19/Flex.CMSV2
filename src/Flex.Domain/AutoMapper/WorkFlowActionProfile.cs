using Flex.Domain.Collections;
using Flex.Domain.Dtos.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.AutoMapper
{
    public class WorkFlowActionProfile : Profile
    {
        public WorkFlowActionProfile()
        {
            CreateMap<sysWorkFlowAction, StepActionButtonDto>();
        }
    }
}
