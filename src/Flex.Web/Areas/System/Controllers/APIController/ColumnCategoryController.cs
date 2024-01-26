using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColumnCategoryController : ApiBaseController
    {
        private IColumnServices _columnServices;
        public ColumnCategoryController(IColumnServices columnServices) {
            _columnServices = columnServices;
        }
        [HttpGet("Column")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<ColumnDto>.getColumnDescList());
        }
        [HttpGet("ColumnDataPermission")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string ColumnDataPermission()
        {
            return Success(ModelTools<ColumnDataPermissionDto>.getColumnDescList());
        }
        /// <summary>
        /// 栏目管理主页面内容
        /// </summary>
        /// <returns></returns>
        [HttpGet("ListAsync")]
        public async Task<string> ListAsync() {
         return Success(await _columnServices.ListAsync());
        }

        /// <summary>
        /// 数据权限分配
        /// </summary>
        /// <returns></returns>
        [HttpGet("DataPermissionListAsync")]
        public async Task<string> DataPermissionListAsync()
        {
            var columns = (await _columnServices.ListAsync()).ToList();
            columns.Add(new ColumnListDto() { Id = -1, ParentId = -2, Name = "快捷选择" });
            return Success(columns.OrderBy(m=>m.Id));
        }

        /// <summary>
        /// 栏目管理下拉框数据【显示的树形结构】
        /// </summary>
        /// <returns></returns>
        [HttpGet("TreeListAsync")]
        [Descriper(Name = "栏目管理下拉框树形数据")]
        public async Task<string> TreeListAsync()
        {
            return Success(await _columnServices.GetTreeColumnListDtos());
        }

        /// <summary>
        /// 内容管理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetManageTreeListAsync")]
        public async Task<string> ManageTreeListAsync()
        {
            return Success(await _columnServices.GetManageTreeListAsync());
        }

        /// <summary>
        /// 栏目管理下拉框数据【隐藏的】
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTreeSelectListDtos")]
        [Descriper(Name = "栏目管理下拉框隐藏数据")]
        public async Task<string> GetTreeSelectListDtos()
        {
            return Success(await _columnServices.GetTreeSelectListDtos());
        }

        [HttpGet("GetColumnById/{Id}")]
        public async Task<string> GetColumnById(int Id) {
            return Success(await _columnServices.GetColumnById(Id));
        }

        [HttpPut]
        public async Task<string> AddColumn() {
            var validate = await ValidateModel<AddColumnDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result =await  _columnServices.AddColumn(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        
        [HttpPost]
        public async Task<string> UpdateColumn() {
            var validate = await ValidateModel<UpdateColumnDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result =await  _columnServices.UpdateColumn(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpDelete("{Id}")]
        public async Task<string> Delete(string Id)
        {
            var result = await _columnServices.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
