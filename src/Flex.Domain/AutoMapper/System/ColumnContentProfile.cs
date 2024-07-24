using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.SignalRBus.Model.Request;

namespace Flex.Domain.AutoMapper.System
{
    public class ColumnContentProfile : Profile
    {
        public ColumnContentProfile()
        {
            CreateMap<ExportRequestModel, ContentPageListParamDto>();
        }
    }
}
