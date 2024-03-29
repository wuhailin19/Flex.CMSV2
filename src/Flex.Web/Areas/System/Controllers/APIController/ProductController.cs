using Flex.Application.Contracts.IServices.Normal;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.Field;
using Flex.Domain.Dtos.Normal.ProductManage;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.system.Controllers.APIController
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "项目管理相关接口")]
    public class ProductController : ApiBaseController
    {
        private IProductManageServices _services;
        public ProductController(IProductManageServices services)
        {
            _services = services;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("ListAsync")]
        [Descriper(Name = "获取项目列表")]
        public async Task<string> ListAsync()
        {
            var result = await _services.GetProjectListAsync();
            return Success(result);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("DetailListAsync")]
        [Descriper(Name = "获取项目修改列表")]
        public async Task<string> DetailListAsync(int id)
        {
            var result = await _services.GetProjectDetailListAsync(id);
            return Success(result);
        }

        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("DetailAsync")]
        [Descriper(Name = "获取项目详情")]
        public async Task<string> DetailAsync(int id)
        {
            var result = await _services.GetProjectDetailAsync(id);
            return Success(result);
        }

        [HttpPost("AddProject")]
        public async Task<string> AddProject()
        {
            var validate = await ValidateModel<AddProjectDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.AddProject(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        [HttpPost("DeleteProItem/{Id}")]
        [Descriper(IsFilter = true)]
        public async Task<string> DeleteProItem(string Id)
        {
            var result = await _services.DeleteProItem(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        [HttpPost("DeleteProDetail/{Id}")]
        [Descriper(IsFilter = true)]
        public async Task<string> DeleteProDetail(string Id)
        {
            var result = await _services.DeleteProDetail(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("AddRecord")]
        public async Task<string> AddRecord()
        {
            var validate = await ValidateModel<AddRecordDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.AddRecord(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        
        [HttpPost("UpdateProject")]
        public async Task<string> UpdateProject()
        {
            var validate = await ValidateModel<UpdateProjectDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _services.UpdateProject(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
    }
}
