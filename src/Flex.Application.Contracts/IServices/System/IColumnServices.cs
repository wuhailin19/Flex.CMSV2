using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.System.Column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IColumnServices
    {
        Task<ProblemDetails<string>> AddColumn(AddColumnDto addColumnDto);
        Task<ProblemDetails<string>> AppendColumn(AppendColumnDto addColumnDto);
        Task<IEnumerable<RoleDataColumnListDto>> DataPermissionListAsync(int siteId);
        Task<ProblemDetails<string>> Delete(string Id);
        Task<UpdateColumnDto> GetColumnById(int Id);
        Task<IEnumerable<TreeColumnListDto>> getColumnShortcut(string mode = "5");
        Task<IEnumerable<ColumnSortListDto>> GetColumnSortListByParentIdAsync(int parentId = 0);
        Task<IEnumerable<TreeColumnListDto>> GetManageTreeListAsync();
        Task<IEnumerable<TreeColumnListDto>> GetTreeColumnListDtos();
        Task<IEnumerable<TreeColumnListDto>> GetTreeSelectListDtos();
        Task<IEnumerable<ColumnListDto>> ListAsync();
        Task<ProblemDetails<string>> QuickEditColumn(ColumnQuickEditDto columnQuickEditDto);
        Task<ProblemDetails<string>> QuickSortColumn(ColumnQuickSortDto columnQuickSortDto);
        Task<ProblemDetails<string>> UpdateColumn(UpdateColumnDto updateColumnDto);
    }
}
