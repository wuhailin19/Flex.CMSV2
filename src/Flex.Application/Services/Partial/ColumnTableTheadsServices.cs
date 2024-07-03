using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices
    {
        public async Task<ColumnPermissionAndTableHeadDto<ColumnContentDto>> GetTableThs(int parentId, int modelId)
        {
            var tableths = ModelTools<ColumnContentDto>.getColumnDescList();

            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == modelId && m.ShowInTable == true)).ToList();

            foreach (var item in fieldmodel)
            {
                tableths.Add(new ModelTools<ColumnContentDto>()
                {
                    title = item.Name,
                    sort = false,
                    align = "center",
                    maxWidth = "200",
                    field = item.FieldName
                });
            }
            var relationtable = await _unitOfWork.GetRepository<sysTableRelation>().GetAllAsync(m => m.ParentModelId == modelId);
            if (relationtable.Count > 0)
            {
                tableths.Add(new ModelTools<ColumnContentDto>()
                {
                    title = "相关信息",
                    sort = false,
                    align = "center",
                    toolbar = "#relationInfo",
                    minWidth = "200",
                    field = "relationInfo"
                });
            }
            ColumnPermissionAndTableHeadDto<ColumnContentDto> columnPermission = new ColumnPermissionAndTableHeadDto<ColumnContentDto>();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(parentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(parentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(parentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(parentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }
        public async Task<ColumnPermissionAndTableHeadDto<HistoryColumnDto>> GetHistoryTableThs(int parentId)
        {
            var tableths = ModelTools<HistoryColumnDto>.getColumnDescList();
            ColumnPermissionAndTableHeadDto<HistoryColumnDto> columnPermission = new ColumnPermissionAndTableHeadDto<HistoryColumnDto>();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(parentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(parentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(parentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(parentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }

        public async Task<ColumnPermissionAndTableHeadDto<SoftDeleteColumnDto>> GetSoftTableThs(int parentId)
        {
            var tableths = ModelTools<SoftDeleteColumnDto>.getColumnDescList();
            ColumnPermissionAndTableHeadDto<SoftDeleteColumnDto> columnPermission = new ColumnPermissionAndTableHeadDto<SoftDeleteColumnDto>();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(parentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(parentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(parentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(parentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }
    }
}
