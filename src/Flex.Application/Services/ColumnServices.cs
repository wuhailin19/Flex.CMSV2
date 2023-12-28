using Flex.Application.Contracts.Exceptions;
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
        public async Task<IEnumerable<TreeColumnListDto>> GetTreeColumnListDtos()
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var list = (await coreRespository.GetAllAsync()).OrderBy(m => m.OrderId).ToList();
          
            List<TreeColumnListDto> treeColumns = new List<TreeColumnListDto>();
            treeColumns.AddRange(_mapper.Map<List<TreeColumnListDto>>(list));
            if (_claims.IsSystem)
            {
                foreach (var item in treeColumns)
                {
                    item.IsDelete = true;
                    item.IsEdit= true;
                    item.IsSelect= true;
                    item.IsAdd= true;
                }
            }

            return treeColumns;
        }
        public async Task<ProblemDetails<string>> AddColumn(AddColumnDto addColumnDto)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var model = _mapper.Map<SysColumn>(addColumnDto);
            AddIntEntityBasicInfo(model);
            try
            {
                coreRespository.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.Message<ErrorCodes>());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
