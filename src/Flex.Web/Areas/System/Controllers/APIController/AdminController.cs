using Flex.Application.Contracts.IServices;
using Flex.Application.Services;
using Flex.Core.Attributes;
using Flex.Core.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ApiBaseController
    {
        private IRoleServices _roleServices;
        private IAdminServices _adminServices;
        private IPictureServices _pictureServices;
        public AdminController(IRoleServices roleServices,IAdminServices adminServices, IPictureServices pictureServices)
        {
            _roleServices = roleServices;
            _adminServices = adminServices;
            _pictureServices = pictureServices;
        }
        /// <summary>
        /// 根据Token获取当前角色权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("Permissions")]
        public async Task<string> GetCurrenAdminPermissions()
        {
            var RoleList = await _roleServices.GetCurrentAdminRoleByTokenAsync();
            if (!RoleList.Any())
                return NotFound();
            return Success(RoleList);
        }
        /// <summary>
        /// 获取当前登录信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLoginInfo")]
        public async Task<string> getLoginInfo()
        {
            var model =await _adminServices.GetCurrentAdminInfoAsync();
            if (model == null)
                return Fail("没有登录");
            return Success(model);
        }
        /// <summary>
        /// 上传账号头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        [Descriper(Name = "上传账号头像")]
        public string OnloadUserAvatar(IFormFileCollection input)
        {
            var result = _pictureServices.UploadImgService(input);
            if (!result.IsSuccess)
                return Fail(result);
            return Success(result);
        }
    }
}
