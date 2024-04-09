using Autofac.Core;
using Flex.Application.Filters;
using Flex.Core.Attributes;
using Flex.Core.Config;
using Flex.Domain.Dtos.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Flex.WebApi.SystemControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Descriper(Name = "账号相关接口")]
    public class AdminController : ApiBaseController
    {
        private IRoleServices _roleServices;
        private IAdminServices _adminServices;
        private IPictureServices _pictureServices;
        public AdminController(IRoleServices roleServices, IAdminServices adminServices, IPictureServices pictureServices)
        {
            _roleServices = roleServices;
            _adminServices = adminServices;
            _pictureServices = pictureServices;
        }
        [HttpGet("Column")]
        [AllowAnonymous]
        [Descriper(IsFilter = true)]
        public string Column()
        {
            return Success(ModelTools<AdminColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        [Descriper(Name = "获取账号列表")]
        public async Task<string> ListAsync(int page, int limit)
        {
            return Success(await _adminServices.GetAdminListAsync(page, limit));
        }
        /// <summary>
        /// 所有Admin信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStepAdminListAsync")]
        [Descriper(IsFilter = true)]
        public async Task<string> GetStepAdminListAsync()
        {
            var list = await _adminServices.GetAsync();
            return Success(list);
        }
        /// <summary>
        /// 根据Token获取当前角色权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("Permissions")]
        [Descriper(Name = "根据Token获取当前角色权限")]
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
        [Descriper(Name = "获取当前登录信息")]
        public async Task<string> getLoginInfo()
        {
            var model = await _adminServices.GetCurrentAdminInfoAsync();
            if (model == null)
                return Fail("没有登录");
            return Success(model);
        }
        /// <summary>
        /// 上传账号头像
        /// </summary>
        /// <returns></returns>
        [HttpPost("OnloadUserAvatar")]
        [Descriper(Name = "上传账号头像")]
        public string OnloadUserAvatar(IFormFileCollection file)
        {
            var result = _pictureServices.UploadImgService(file);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(ServerConfig.ImageServerUrl + result.Detail);
        }

        /// <summary>
        /// 编辑账号基本资料
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateUserAvatar")]
        [Descriper(Name = "编辑账号基本资料")]
        public async Task<string> UpdateUserAvatar()
        {
            var validate = await ValidateModel<SimpleEditAdminDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _adminServices.SimpleEditAdminUpdate(validate.Content);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        /// <summary>
        /// 新增账号
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateAccount")]
        [Descriper(Name = "新增账号")]
        public async Task<string> Add()
        {
            var validate = await ValidateModel<AdminAddDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _adminServices.InsertAdmin(validate.Content);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        /// <summary>
        /// 通过账号Id获取账号信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetEditDtoInfoById/{Id}")]
        [Descriper(Name = "通过账号Id获取账号信息")]
        public async Task<string> GetEditDtoInfo(string Id)
        {
            var model = await _adminServices.GetEditDtoInfoByIdAsync(Id.ToLong());
            if (model == null)
                return NotFound();
            return Success(model);
        }
        /// <summary>
        /// 修改账号信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Descriper(Name = "修改账号信息")]
        public async Task<string> Update()
        {
            var validate = await ValidateModel<AdminEditDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _adminServices.UpdateAdmin(validate.Content);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        /// <summary>
        /// 修改账号密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdatePassword")]
        [Descriper(Name = "修改账号密码")]
        public async Task<string> UpdatePassword()
        {
            var validate = await ValidateModel<AccountAndPasswordDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _adminServices.UpdateCurrentAccountPassword(validate.Content);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("{Id}")]
        [Descriper(Name = "删除账号")]
        public async Task<string> Delete(string Id)
        {
            var result = await _adminServices.DeleteAccountListByIdArray(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        [HttpPost("QuickEdit")]
        [Descriper(Name = "快速修改账号状态")]
        public async Task<string> QuickEdit()
        {
            AdminQuickEditDto adminQuickEdit = await GetModel<AdminQuickEditDto>();
            var result = await _adminServices.QuickEditAdmin(adminQuickEdit);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
