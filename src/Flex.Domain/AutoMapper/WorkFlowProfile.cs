using Flex.Domain.Collections;
using Flex.Domain.Dtos.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.AutoMapper
{
    public class WorkFlowProfile : Profile
    {
        public WorkFlowProfile()
        {
            CreateMap<InputWorkFlowAddDto, sysWorkFlow>();
            CreateMap<WorkFlowDto, sysWorkFlow>()
                    .ForMember(a => a.Id, opt => opt.MapFrom(b => b.Id))
                    .ForMember(a => a.WorkFlowContent, opt => opt.MapFrom(b => b.WorkFlowContent))
                    .ForMember(a => a.stepDesign, opt => opt.MapFrom(b => b.stepDesign))
                    .ForMember(a => a.actionString, opt => opt.MapFrom(b => b.actionString))
                    .ForMember(a => a.actDesign, opt => opt.MapFrom(b => b.actDesign));
            CreateMap<sysWorkFlow, WorkFlowColumnDto>();
            CreateMap<PagedList<sysWorkFlow>, PagedList<WorkFlowColumnDto>>();

        }
    }
}
