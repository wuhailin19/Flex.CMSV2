using Flex.Application.Contracts.Aop;
using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.System.SiteManage;
using Flex.EFSql.Repositories;

namespace Flex.Application.Services.System
{
    public class SiteManageServices : BaseService, ISiteManageServices
    {
        IEfCoreRespository<sysSiteManage> responsity;
        public SiteManageServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            responsity = _unitOfWork.GetRepository<sysSiteManage>();
        }

        public async Task<IEnumerable<SiteManageColumnDto>> ListAsync()
        {
            var list = (await responsity.GetAllAsync()).ToList();
            return _mapper.Map<List<SiteManageColumnDto>>(list);
        }
        public async Task<ProblemDetails<string>> AddSite(AddSiteManageDto addmodel)
        {
            var Repository = _unitOfWork.GetRepository<sysSiteManage>();
            var model = _mapper.Map<sysSiteManage>(addmodel);
            AddIntEntityBasicInfo(model);
            try
            {
                Repository.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataInsertError.GetEnumDescription(),ex);
            }
        }
        public async Task<ProblemDetails<string>> UpdateSite(UpdateSiteManageDto updatemodel)
        {
            var Repository = _unitOfWork.GetRepository<sysSiteManage>();
            try
            {
                var model = await Repository.GetFirstOrDefaultAsync(m => m.Id == updatemodel.Id);
                _mapper.Map(updatemodel, model);
                UpdateIntEntityBasicInfo(model);
                _unitOfWork.GetRepository<sysSiteManage>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch(Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            var Repository = _unitOfWork.GetRepository<sysSiteManage>();
            if (Id.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = Repository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<sysSiteManage>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                }
                Repository.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch(Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(), ex);
                //throw;
            }
        }
    }
}
