using Flex.Application.Contracts.IServices;
using Flex.Application.Services;
using Flex.Core.Attributes;
using Flex.Core.Helper;
using Flex.Domain.Dtos.Admin;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Flex.Web.Areas.System.Controllers.APIController
{
	[Route("api/[controller]")]
	[ApiController]
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
		[Descriper(IsFilter = true)]
		public string Column()
		{
			return Success(ModelTools<AdminColumnDto>.getColumnDescList());
		}
		[HttpGet("ListAsync")]
		[Descriper(Name = "获取AdminList")]
		public async Task<string> ListAsync(int page, int limit)
		{
			return Success(await _adminServices.GetAdminListAsync(page, limit));
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
			return Success(result.Detail);
		}

		/// <summary>
		/// 编辑账号基本资料
		/// </summary>
		/// <returns></returns>
		[HttpPost("UpdateUserAvatar")]
		[Descriper(Name = "编辑账号基本资料")]
		public async Task<string> UpdateUserAvatar()
		{
			SimpleEditAdminDto simpleEditAdmin = await GetModel<SimpleEditAdminDto>();
			var result = await _adminServices.SimpleEditAdminUpdate(simpleEditAdmin);
			if (result.IsSuccess)
				return Success(result.Detail);
			return Fail(result.Detail);
		}
		/// <summary>
		/// 新增账号
		/// </summary>
		/// <returns></returns>
		[HttpPut]
		public async Task<string> Add()
		{
            AdminAddDto adminEditDto = await GetModel<AdminAddDto>();
			// 使用验证器验证实体
			var validationContext = new ValidationContext(adminEditDto);
			var validationResults = new List<ValidationResult>();
			bool isValid = Validator.TryValidateObject(adminEditDto, validationContext, validationResults, true);
			if (!isValid)
			{
				return Fail(validationResults[0]?.ErrorMessage ?? "验证未通过");
			}
			var result = await _adminServices.InsertAdmin(adminEditDto);
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
		public async Task<string> GetEditDtoInfo(string Id) {
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
        public async Task<string> Update()
        {
            AdminEditDto adminEditDto = await GetModel<AdminEditDto>();
            // 使用验证器验证实体
            var validationContext = new ValidationContext(adminEditDto);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(adminEditDto, validationContext, validationResults, true);
            if (!isValid)
            {
                return Fail(validationResults[0]?.ErrorMessage ?? "验证未通过");
            }
            var result = await _adminServices.UpdateAdmin(adminEditDto);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        /// <summary>
        /// 修改账号密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdatePassword")]
        public async Task<string> UpdatePassword()
        {
            AccountAndPasswordDto accountmodel = await GetModel<AccountAndPasswordDto>();
            // 使用验证器验证实体
            var validationContext = new ValidationContext(accountmodel);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(accountmodel, validationContext, validationResults, true);
            if (!isValid)
            {
                return Fail(validationResults[0]?.ErrorMessage ?? "验证未通过");
            }
            var result = await _adminServices.UpdateCurrentAccountPassword(accountmodel);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

		[HttpDelete("{Id}")]
		public async Task<string> Delete(string Id)
		{
            var result = await _adminServices.DeleteAccountListByIdArray(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
		[HttpPost("QuickEdit")]
        public async Task<string> QuickEdit()
        {
            AdminQuickEditDto adminQuickEdit= await GetModel<AdminQuickEditDto>();
            var result = await _adminServices.QuickEditAdmin(adminQuickEdit);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
