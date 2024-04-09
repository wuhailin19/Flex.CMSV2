using Flex.Core.Attributes;
using Flex.Core.Helper;
using Flex.Core.Reflection;
using Flex.Core.Timing;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.WebApi.SystemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "接口地址相关接口")]
    public class RoleUrlController : ApiBaseController
    {
        private IRoleUrlServices _roleUrlServices;
        public RoleUrlController(IRoleUrlServices roleUrlServices)
        {
            _roleUrlServices = roleUrlServices;
        }

        [HttpGet("GetRoleUrlListById")]
        [Descriper(Name = "通过角色Id获取接口列表数据")]
        public async Task<string> GetRoleUrlListById(int Id)
        {
            var result = await _roleUrlServices.GetRoleUrlListById(Id);
            return Success(result);
        }

        /// <summary>
        /// 获取接口列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("ListAsync")]
        [Descriper(Name = "获取接口列表", Desc = "参数\r\ncateid：1为数据接口，2为视图接口")]
        public async Task<string> ListAsync(string cateid, string? k = null)
        {
            var result = await _roleUrlServices.GetApiUrlListByCateId(cateid, k);
            return Success(result);
        }

        /// <summary>
        /// 遍历所有接口地址并存入数据库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Descriper(Name = "遍历所有接口地址并存入数据库")]
        public async Task<string> InitRoleUrl()
        {
            var result = await _roleUrlServices.CreateUrlList();
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }


    }
}
