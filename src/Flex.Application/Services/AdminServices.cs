using AutoMapper;
using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos;
using Flex.Domain.Dtos.Role;
using Org.BouncyCastle.Crypto;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;

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

        public async Task<SimpleAdminDto> GetCurrentAdminInfoAsync() =>
          _mapper.Map<SimpleAdminDto>(
              await GetAdminById(_claims.UserId));

        public async Task<AdminEditInfoDto> GetEditDtoInfoByIdAsync(long Id) =>
          _mapper.Map<AdminEditInfoDto>(
              await GetAdminById(Id));

        public async Task<ProblemDetails<string>> SimpleEditAdminUpdate(SimpleEditAdminDto simpleEditAdmin)
        {
            var model = await GetAdminById(simpleEditAdmin.Id);
            if (model.Version != simpleEditAdmin.Version)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataVersionError.Message<ErrorCodes>());
            }
            _mapper.Map(simpleEditAdmin, model);
            model.LastEditDate = Clock.Now;
            model.LastEditUser = _claims.UserId;
            model.LastEditUserName = _claims.UserName;
            model.Version += 1;
            _unitOfWork.GetRepository<SysAdmin>().Update(model);
            await _unitOfWork.SaveChangesAsync();
            return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.Message<ErrorCodes>());
        }

        public async Task<ProblemDetails<string>> InsertAdmin(AdminAddDto insertAdmin)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();

            var model = await adminRepository.GetFirstOrDefaultAsync(m => m.Account == insertAdmin.Account, null, null, true, false);
            if (model != null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "该账号已存在");

            var saltvalue = SaltStringHelper.getSaltStr();
            model = _mapper.Map<SysAdmin>(insertAdmin);
            model.Id = _idWorker.NextId();
            model.SaltValue = saltvalue;
            model.Password = EncryptHelper.MD5Encoding(insertAdmin.Password, model.SaltValue);
            model.AddUser = _claims.UserId;
            model.AddUserName = _claims.UserName;
            model.Mutiloginccode = saltvalue;
            var result = await adminRepository.InsertAsync(model);
            await _unitOfWork.SaveChangesAsync();
            if (result.Entity.Id > 0)
                return new ProblemDetails<string>(HttpStatusCode.OK, "添加成功");
            else
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.Message<ErrorCodes>());
        }

        public async Task<ProblemDetails<string>> UpdateAdmin(AdminEditDto editAdmin)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            var model = await GetAdminById(editAdmin.Id);
            if (model.Version != editAdmin.Version)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataVersionError.Message<ErrorCodes>());
            }
            if (model.Account != editAdmin.Account)
            {
                var checkmodel = await adminRepository.GetFirstOrDefaultAsync(m => m.Account == editAdmin.Account);
                if (checkmodel != null)
                    return new ProblemDetails<string>(HttpStatusCode.BadRequest, "该账号已存在");
            }
            var saltvalue = SaltStringHelper.getSaltStr();
            _mapper.Map(editAdmin, model);
            if (editAdmin.Password.IsNotNullOrEmpty())
            {
                model.SaltValue = saltvalue;
                model.Password = EncryptHelper.MD5Encoding(editAdmin.Password, model.SaltValue);
            }
            model.LastEditUser = _claims.UserId;
            model.LastEditUserName = _claims.UserName;
            model.LastEditDate = Clock.Now;
            model.Version += 1;
            try
            {
                adminRepository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.Message<ErrorCodes>());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.Message<ErrorCodes>());
            }
        }

        public async Task<ProblemDetails<string>> UpdateCurrentAccountPassword(AccountAndPasswordDto accountAndPasswordDto)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            var model = await GetAdminById(_claims.UserId);
            if (model.Version != accountAndPasswordDto.Version)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataVersionError.Message<ErrorCodes>());
            }
            if (accountAndPasswordDto.Account != model.Account)
            {
                var checkmodel = await adminRepository.GetFirstOrDefaultAsync(m => m.Account == accountAndPasswordDto.Account);
                if (checkmodel != null)
                    return new ProblemDetails<string>(HttpStatusCode.BadRequest, "该账号已存在");
            }
            model.Account = accountAndPasswordDto.Account;
            var saltvalue = SaltStringHelper.getSaltStr();
            if (accountAndPasswordDto.Password.IsNotNullOrEmpty())
            {
                model.SaltValue = saltvalue;
                model.Password = EncryptHelper.MD5Encoding(accountAndPasswordDto.Password, model.SaltValue);
            }
            model.LastEditUser = _claims.UserId;
            model.LastEditUserName = _claims.UserName;
            model.LastEditDate = Clock.Now;
            model.Version += 1;
            try
            {
                adminRepository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.Message<ErrorCodes>());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.Message<ErrorCodes>());
            }
        }

        public async Task<ProblemDetails<string>> DeleteAccountListByIdArray(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            if (Id.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "未选择删除数据");
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            try
            {
                adminRepository.Delete(delete_list);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "删除失败");
            }
        }

        public async Task<ProblemDetails<string>> QuickEditAdmin(AdminQuickEditDto adminQuickEditDto)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            var model = await adminRepository.GetFirstOrDefaultAsync(m=>m.Id==adminQuickEditDto.Id);
            if(model is null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.Message<ErrorCodes>());
            if(adminQuickEditDto.AllowMultiLogin.IsNotNullOrEmpty())
                model.AllowMultiLogin= adminQuickEditDto.AllowMultiLogin.CastTo<bool>();
            if (adminQuickEditDto.Islock.IsNotNullOrEmpty())
                model.Islock = adminQuickEditDto.Islock.CastTo<bool>();
            try
            {
                adminRepository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.Message<ErrorCodes>());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.Message<ErrorCodes>());
            }
        }
    }
}
