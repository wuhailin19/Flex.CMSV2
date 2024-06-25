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
        public async Task<ColumnPermissionAndTableHeadDto<ColumnContentDto>> GetTableThs(int ParentId)
        {
            var tableths = ModelTools<ColumnContentDto>.getColumnDescList();
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId && m.ShowInTable == true)).ToList();
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
            ColumnPermissionAndTableHeadDto<ColumnContentDto> columnPermission = new ColumnPermissionAndTableHeadDto<ColumnContentDto>();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(ParentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(ParentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(ParentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(ParentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }
        public async Task<ColumnPermissionAndTableHeadDto<HistoryColumnDto>> GetHistoryTableThs(int ParentId)
        {
            var tableths = ModelTools<HistoryColumnDto>.getColumnDescList();
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            ColumnPermissionAndTableHeadDto<HistoryColumnDto> columnPermission = new ColumnPermissionAndTableHeadDto<HistoryColumnDto>();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(ParentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(ParentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(ParentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(ParentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }

        public async Task<ColumnPermissionAndTableHeadDto<SoftDeleteColumnDto>> GetSoftTableThs(int ParentId)
        {
            var tableths = ModelTools<SoftDeleteColumnDto>.getColumnDescList();
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            ColumnPermissionAndTableHeadDto<SoftDeleteColumnDto> columnPermission = new ColumnPermissionAndTableHeadDto<SoftDeleteColumnDto>();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(ParentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(ParentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(ParentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(ParentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }
    }
}
