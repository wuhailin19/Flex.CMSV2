using Flex.Domain.Base;

namespace Flex.Application.Services
{
    public class ColumnContentServices : BaseService, IColumnContentServices
    {
        public ColumnContentServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims) 
            : base(unitOfWork, mapper, idWorker, claims)
        {
            
        }
        //public async Task<IEnumerable<NomalContentModel>> ListAsync(int ParentId) {

        //    var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
        //    var field = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId)).ToList();
         
        //}
    }
}
