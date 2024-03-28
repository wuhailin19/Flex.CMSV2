using Flex.Domain.Dtos.Normal.ProductManage;
using Flex.Domain.Entities.Normal;

namespace Flex.Domain.AutoMapper.Project
{
    public class ProductManageProfile : Profile
    {
        public ProductManageProfile()
        {
            CreateMap<AddProjectDto, norProductManage>();
            CreateMap<norProductManage, ProjectListDto>();
            CreateMap<norProductManage, ProjectDetailDto>();

            CreateMap<norProductDetail, ProductDetailListDto>();
            CreateMap<AddRecordDto, norProductDetail>();
        }
    }
}
