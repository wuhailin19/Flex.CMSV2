using Flex.Core.Helper;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.AutoMapper
{
    public class FiledProfile: Profile
    {
        public FiledProfile() {
            CreateMap<sysField, FieldColumnDto>();
            CreateMap<sysField, UpdateFieldDto>()
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => DeserializeAttritude(src.FieldAttritude).Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => DeserializeAttritude(src.FieldAttritude).Height))
                .ForMember(dest => dest.ValidateEmpty, opt => opt.MapFrom(src => DeserializeValidation(src.Validation).ValidateEmpty))
                .ForMember(dest => dest.ValidateNumber, opt => opt.MapFrom(src => DeserializeValidation(src.Validation).ValidateNumber));
              
            CreateMap<AddFieldDto, sysField>();
        }
        private FieldAttritudeModel DeserializeAttritude(string loginLogString)
        {
            // 这里使用你的反序列化逻辑，例如使用 JsonHelper.Json<T>
            // 示例中仅为演示目的，实际实现可能会有所不同
            return JsonHelper.Json<FieldAttritudeModel>(loginLogString);
        }
        private FiledValidateModel DeserializeValidation(string loginLogString)
        {
            // 这里使用你的反序列化逻辑，例如使用 JsonHelper.Json<T>
            // 示例中仅为演示目的，实际实现可能会有所不同
            return JsonHelper.Json<FiledValidateModel>(loginLogString);
        }
    }
}
