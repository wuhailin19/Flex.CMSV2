using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices.Normal;
using Flex.Domain.Dtos.Normal.ProductManage;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Entities.Normal;

namespace Flex.Application.Services.Normal
{
    public class ProductManageServices : BaseService, IProductManageServices
    {
        public ProductManageServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {

        }
        /// <summary>
        /// 获取项目详情列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<ProductDetailListDto>> GetProjectDetailListAsync(int projectid)
        {
            Func<IQueryable<norProductDetail>, IOrderedQueryable<norProductDetail>> orderfunc = m => m.OrderByDescending(m => m.AddTime);
            var list = await _unitOfWork.GetRepository<norProductDetail>().GetAllAsync(m => m.ProjectId == projectid, orderfunc);
            var nopagelist
                = _mapper
                .Map<List<ProductDetailListDto>>(list);
            return nopagelist;
        }
        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<ProjectDetailDto> GetProjectDetailAsync(int id)
        {
            var model = await _unitOfWork.GetRepository<norProductManage>().GetFirstOrDefaultAsync(m => m.Id == id);
            var detail
                = _mapper
                .Map<ProjectDetailDto>(model);
            return detail;
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<ProjectListDto>> GetProjectListAsync()
        {
            Func<IQueryable<norProductManage>, IOrderedQueryable<norProductManage>> orderfunc = m => m.OrderBy(m => m.ProductName);
            var list = await _unitOfWork.GetRepository<norProductManage>().GetAllAsync(null, orderfunc);
            var nopagelist
                = _mapper
                .Map<List<ProjectListDto>>(list);
            return nopagelist;
        }
        public async Task<ProblemDetails<string>> AddProject(AddProjectDto addColumnDto)
        {
            var coreRespository = _unitOfWork.GetRepository<norProductManage>();
            var model = _mapper.Map<norProductManage>(addColumnDto);
            AddIntEntityBasicInfo(model);
            try
            {
                coreRespository.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
        }
        public async Task<ProblemDetails<string>> UpdateProject(UpdateProjectDto addColumnDto)
        {
            var coreRespository = _unitOfWork.GetRepository<norProductManage>();
            var model =await coreRespository.GetFirstOrDefaultAsync(m=>m.Id== addColumnDto.Id);
            model.ServerInfo = addColumnDto.ServerInfo;
            model.ProductName=addColumnDto.ProductName;
            model.Participants=addColumnDto.Participants;
            UpdateIntEntityBasicInfo(model);
            try
            {
                coreRespository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
        }
        public async Task<ProblemDetails<string>> DeleteProDetail(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<norProductDetail>();
            if (Id.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<norProductDetail>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                }
                adminRepository.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch
            {
                throw;
            }
        }
        public async Task<ProblemDetails<string>> DeleteProItem(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<norProductManage>();
            if (Id.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<norProductManage>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                }
                adminRepository.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch
            {
                throw;
            }
        }
        public async Task<ProblemDetails<string>> AddRecord(AddRecordDto addColumnDto)
        {
            var coreRespository = _unitOfWork.GetRepository<norProductDetail>();
            var model = _mapper.Map<norProductDetail>(addColumnDto);
            AddIntEntityBasicInfo(model);
            try
            {
                coreRespository.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
        }
    }
}
