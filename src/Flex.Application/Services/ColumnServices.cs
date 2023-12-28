using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class ColumnServices : BaseService, IColumnServices
    {
        public ColumnServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
        private void AddTreeColumn(List<SysColumn> fulllist, List<TreeColumnListDto> treeColumns)
        {
            List<SysColumn> childrens = new List<SysColumn>();
            foreach (var item in treeColumns)
            {
                childrens = fulllist.Where(m => m.ParentId == item.Id).ToList();
                if (childrens != null)
                {
                    item.children = _mapper.Map<List<TreeColumnListDto>>(childrens);
                    AddTreeColumn(fulllist, treeColumns);
                }
            }
        }
        public async Task<IEnumerable<TreeColumnListDto>> GetTreeColumnListDtos()
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var list = (await coreRespository.GetAllAsync()).OrderBy(m => m.OrderId).ToList();
            if (!_claims.IsSystem)
            {
                //非超管情况
            }
            List<TreeColumnListDto> treeColumns = new List<TreeColumnListDto>();
            treeColumns.AddRange(_mapper.Map<List<TreeColumnListDto>>(list.Where(m => m.ParentId == 0)));

            AddTreeColumn(list, treeColumns);
            return treeColumns;
        }
        public async Task<ProblemDetails<string>> AddColumn() {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();

            return null;
        }


    }
}
