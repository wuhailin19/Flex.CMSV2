using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Normal.ProductManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices.Normal
{
    public interface IProductManageServices
    {
        Task<ProblemDetails<string>> AddProject(AddProjectDto addColumnDto);
        Task<ProblemDetails<string>> AddRecord(AddRecordDto addColumnDto);
        Task<ProblemDetails<string>> DeleteProDetail(string Id);
        Task<ProblemDetails<string>> DeleteProItem(string Id);
        Task<ProjectDetailDto> GetProjectDetailAsync(int id);
        Task<List<ProductDetailListDto>> GetProjectDetailListAsync(int projectid);
        Task<List<ProjectListDto>> GetProjectListAsync();
        Task<ProblemDetails<string>> UpdateProject(UpdateProjectDto addColumnDto);
    }
}
