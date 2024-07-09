using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.System.Column;
using Flex.Domain.Dtos.System.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Flex.WebApi.SystemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(Name = "栏目管理相关接口")]
    public class ColumnCategoryController : ApiBaseController
    {
        private IColumnServices _columnServices;
        public ColumnCategoryController(IColumnServices columnServices)
        {
            _columnServices = columnServices;
        }
        [HttpGet("Column")]
        [Descriper(IsFilter = true)]
        [AllowAnonymous]
        public string Column()
        {
            return SuccessIgnoreNull(ModelTools<ColumnDto>.getColumnDescList());
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
        [Descriper(Name = "栏目管理页面列表数据")]
        public async Task<string> ListAsync()
        {
            return Success(await _columnServices.ListAsync());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetColumnSortListByParentIdAsync/{parentId}")]
        [Descriper(Name = "栏目快速排序列表数据")]
        public async Task<string> GetColumnSortListByParentIdAsync(int parentId)
        {
            return Success(await _columnServices.GetColumnSortListByParentIdAsync(parentId));
        }


        /// <summary>
        /// 获取栏目快捷方式
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpGet("getColumnShortcut")]
        [Descriper(IsFilter = true)]
        public async Task<string> getColumnShortcut(string mode = "5")
        {
            return Success(await _columnServices.getColumnShortcut(mode));
        }

        /// <summary>
        /// 数据权限分配
        /// </summary>
        /// <returns></returns>
        [HttpGet("DataPermissionListAsync")]
        [Descriper(Name = "数据权限分配页面数据")]
        public async Task<string> DataPermissionListAsync(int siteId)
        {
            var columns = (await _columnServices.DataPermissionListAsync(siteId)).ToList();
            columns.Add(new RoleDataColumnListDto() { Id = -10000, ParentId = -2, Name = "快捷选择" });
            return Success(columns.OrderBy(m => m.Id));
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
        /// 栏目管理下拉框数据【显示的树形结构】
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTreeListBySiteIdAsync")]
        [Descriper(Name = "根据站点Id和模型Id筛选可复制的栏目")]
        public async Task<string> GetTreeListBySiteIdAsync(int siteId,int modelId)
        {
            return Success(await _columnServices.GetTreeListBySiteIdAsync(siteId, modelId));
        }

        /// <summary>
        /// 内容管理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetManageTreeListAsync")]
        [Descriper(Name = "内容管理页面栏目树形数据")]
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
        [Descriper(Name = "通过Id获取栏目数据")]
        public async Task<string> GetColumnById(int Id)
        {
            return Success(await _columnServices.GetColumnById(Id));
        }

        [HttpPost("CreateColumn")]
        [Descriper(Name = "新增栏目")]
        public async Task<string> AddColumn()
        {
            var validate = await ValidateModel<AddColumnDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _columnServices.AddColumn(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost("AppendColumn")]
        [Descriper(Name = "批量添加栏目")]
        public async Task<string> AppendColumn()
        {
            var validate = await ValidateModel<AppendColumnDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _columnServices.AppendColumn(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }

        [HttpPost]
        [Descriper(Name = "修改栏目")]
        public async Task<string> UpdateColumn()
        {
            var validate = await ValidateModel<UpdateColumnDto>();
            if (!validate.IsSuccess)
                return Fail(validate.Detail);
            var result = await _columnServices.UpdateColumn(validate.Content);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        [HttpPost("QuickEdit")]
        [Descriper(Name = "快速修改栏目状态")]
        public async Task<string> QuickEdit()
        {
            var model = await ValidateModel<ColumnQuickEditDto>();
            if (!model.IsSuccess)
                return Fail(model.Detail);
            var result = await _columnServices.QuickEditColumn(model.Content);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        [HttpPost("QuickSortColumn")]
        [Descriper(Name = "快速栏目排序")]
        public async Task<string> QuickSortColumn()
        {
            var model = await GetModel<ColumnQuickSortDto>();
            var result = await _columnServices.QuickSortColumn(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        [HttpPost("{Id}")]
        [Descriper(Name = "删除栏目")]
        public async Task<string> Delete(string Id)
        {
            var result = await _columnServices.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
