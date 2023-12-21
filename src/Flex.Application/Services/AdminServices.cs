using AutoMapper;
using Flex.Domain.Dtos;
using Flex.Domain.Dtos.Role;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

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
        /// 获取账号列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<PagedList<AdminColumnDto>> GetAdminListAsync(int page, int pagesize)
        {
            var list = await _unitOfWork.GetRepository<SysAdmin>().GetPagedListAsync(m => m.RoleId != 0, null, null, page, pagesize);
            PagedList<AdminColumnDto> model = _mapper
                .Map<PagedList<AdminColumnDto>>(list);
            return model;
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

        public async Task<ProblemDetails<string>> SimpleEditAdminUpdate(SimpleEditAdminDto simpleEditAdmin)
        {
            var model = await GetAdminById(simpleEditAdmin.Id);
            if (model.Version == simpleEditAdmin.Version)
            {
                _mapper.Map(simpleEditAdmin, model);
                model.LastEditDate = Clock.Now;
                model.LastEditUser = _claims.UserId;
                model.LastEditUserName = _claims.UserName;
                model.Version += 1;
                _unitOfWork.GetRepository<SysAdmin>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, "修改成功");
            }
            return new ProblemDetails<string>(HttpStatusCode.BadRequest, "重复修改");
        }

        public async Task<ProblemDetails<string>> InsertAdmin(AdminEditDto insertAdmin)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();

            var model = await adminRepository.GetFirstOrDefaultAsync(m => m.Account == insertAdmin.Account, null, null, true, false);
            if (model != null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "该账号已存在");

            var saltvalue = SaltStringHelper.getSaltStr();
            model = _mapper.Map<SysAdmin>(insertAdmin);
            model.Id = _idWorker.NextId();
            model.SaltValue = saltvalue;
            model.Password = EncryptHelper.MD5Encoding(model.Password, model.SaltValue);
            model.AddUser = _claims.UserId;
            model.Mutiloginccode = saltvalue;
            var result = await adminRepository.InsertAsync(model);
            await _unitOfWork.SaveChangesAsync();
            if (result.Entity.Id > 0)
                return new ProblemDetails<string>(HttpStatusCode.OK, "添加成功");
            else
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "添加失败");
        }
    }
}
