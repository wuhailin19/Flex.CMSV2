using Flex.Domain.Dtos;

namespace Flex.Application.Services
{
    public class AdminServices : BaseService, IAdminServices
    {
        public AdminServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
        public async Task<IEnumerable<AdminDto>> GetAsync()
        {
            var admin_list = await _unitOfWork.GetRepository<SysAdmin>().GetAllAsync();
            return _mapper.Map<IEnumerable<AdminDto>>(admin_list);
        }
        public async Task<PagedList<AdminDto>> GetPageListAsync(int pagesize = 10)
        {
            Func<IQueryable<SysAdmin>, IOrderedQueryable<SysAdmin>> orderby
                = new Func<IQueryable<SysAdmin>, IOrderedQueryable<SysAdmin>>(m => m.OrderByDescending(m => m.AddTime));
            var admin_list = await _unitOfWork
                .GetRepository<SysAdmin>()
                .GetPagedListAsync(null, orderby, null, 1, pagesize);
            return _mapper.Map<PagedList<AdminDto>>(admin_list);
        }
        /// <summary>
        /// 根据ID获取Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserData> GetAdminValidateInfoAsync(long id) =>
          _mapper.Map<UserData>(
              await GetAdminById(id));

        public async Task<SysAdmin> GetAdminById(long id) =>
             await _unitOfWork.GetRepository<SysAdmin>().GetFirstOrDefaultAsync(m => m.Id == id, null, null, true, false);

        /// <summary>
        /// 获取当前Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SimpleAdminDto> GetCurrentAdminInfoAsync() =>
          _mapper.Map<SimpleAdminDto>(
              await _unitOfWork.GetRepository<SysAdmin>().GetFirstOrDefaultAsync(m => m.Id == _claims.UserId, null, null, true, false));

        public async Task<ProblemDetails<string>> SimpleEditAdminUpdate(SimpleAdminDto simpleEditAdmin)
        {
            var model = await GetAdminById(simpleEditAdmin.Id);
            if (model.Version == simpleEditAdmin.Version)
            {
                model = _mapper.Map<SysAdmin>(simpleEditAdmin);
                model.LastEditDate = Clock.Now;
                model.LastEditUser = _claims.UserId;
                model.LastEditUserName = _claims.UserName;
                model.Version += 1;
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, "修改成功");
            }
            return new ProblemDetails<string>(HttpStatusCode.BadRequest, "修改版本不一致");
        }
    }
}
