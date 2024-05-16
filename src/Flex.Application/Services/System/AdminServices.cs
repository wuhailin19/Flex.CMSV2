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
            var dtolist = _mapper.Map<IEnumerable<AdminDto>>(admin_list).ToList();
            dtolist.Add(new AdminDto
            {
                Id = "00000",
                UserName = "快捷选择"
            });
            return dtolist.OrderBy(m => m.Id);
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
        /// <param name="pagesize"></param>
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
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataVersionError.GetEnumDescription());
            }
            _mapper.Map(simpleEditAdmin, model);
            UpdateLongEntityBasicInfo(model);
            _unitOfWork.GetRepository<SysAdmin>().Update(model);
            await _unitOfWork.SaveChangesAsync();
            return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
        }

        public async Task<ProblemDetails<string>> InsertAdmin(AdminAddDto insertAdmin)
        {
            //禁止新增超管
            if (insertAdmin.RoleId == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            var model = await adminRepository.GetFirstOrDefaultAsync(m => m.Account == insertAdmin.Account, null, null, true, false);
            if (model != null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.AccountExist.GetEnumDescription());
            var saltvalue = SaltStringHelper.getSaltStr();
            model = _mapper.Map<SysAdmin>(insertAdmin);

            model.SaltValue = saltvalue;
            model.Password = EncryptHelper.MD5Encoding(insertAdmin.Password, model.SaltValue);

            AddLongEntityBasicInfo(model);
            model.Mutiloginccode = saltvalue;

            if (insertAdmin.PwdExpiredTime.ToInt() != 0)
            {
                model.PwdUpdateTime = Clock.Now.AddDays(insertAdmin.PwdExpiredTime.ToInt());
                model.PwdExpiredTime = insertAdmin.PwdExpiredTime.ToString();
            }
            var result = await adminRepository.InsertAsync(model);
            await _unitOfWork.SaveChangesAsync();
            if (result.Entity.Id > 0)
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            else
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
        }

        public async Task<ProblemDetails<string>> UpdateAdmin(AdminEditDto editAdmin)
        {
            //禁止新增超管
            if (editAdmin.RoleId == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            var model = await GetAdminById(editAdmin.Id);
            if (model.Version != editAdmin.Version)
            {
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataVersionError.GetEnumDescription());
            }
            if (model.Account != editAdmin.Account)
            {
                var checkmodel = await adminRepository.GetFirstOrDefaultAsync(m => m.Account == editAdmin.Account);
                if (checkmodel != null)
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.AccountExist.GetEnumDescription());
            }
            var saltvalue = SaltStringHelper.getSaltStr();
            _mapper.Map(editAdmin, model);

            if (editAdmin.Password.IsNotNullOrEmpty())
            {
                model.SaltValue = saltvalue;
                model.Password = EncryptHelper.MD5Encoding(editAdmin.Password, model.SaltValue);
            }
            //重置密码修改时间
            if (editAdmin.PwdExpiredTime.ToInt() != 0)
            {
                model.PwdUpdateTime = Clock.Now.AddDays(editAdmin.PwdExpiredTime.ToInt());
                model.PwdExpiredTime = editAdmin.PwdExpiredTime.ToString();
            }
            else
            {
                model.PwdUpdateTime = null;
                model.PwdExpiredTime = null;
            }
            UpdateLongEntityBasicInfo(model);
            try
            {
                adminRepository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> UpdateCurrentAccountPassword(AccountAndPasswordDto accountAndPasswordDto)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            var model = await GetAdminById(_claims.UserId);
            if (model.Version != accountAndPasswordDto.Version)
            {
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataVersionError.GetEnumDescription());
            }
            if (accountAndPasswordDto.Account != model.Account)
            {
                var checkmodel = await adminRepository.GetFirstOrDefaultAsync(m => m.Account == accountAndPasswordDto.Account);
                if (checkmodel != null)
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.AccountExist.GetEnumDescription());
            }
            model.Account = accountAndPasswordDto.Account;
            var saltvalue = SaltStringHelper.getSaltStr();
            if (accountAndPasswordDto.Password.IsNotNullOrEmpty())
            {
                model.SaltValue = saltvalue;
                model.Password = EncryptHelper.MD5Encoding(accountAndPasswordDto.Password, model.SaltValue);
                var expiredtime = model.PwdExpiredTime.ToInt();
                if (expiredtime != 0)
                {
                    model.PwdUpdateTime = Clock.Now.AddDays(expiredtime);
                }
            }
            UpdateLongEntityBasicInfo(model);
            try
            {
                adminRepository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> DeleteAccountListByIdArray(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            if (Id.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<SysAdmin>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateLongEntityBasicInfo(item);
                    softdels.Add(item);
                }

                adminRepository.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> QuickEditAdmin(AdminQuickEditDto adminQuickEditDto)
        {
            var adminRepository = _unitOfWork.GetRepository<SysAdmin>();
            var model = await adminRepository.GetFirstOrDefaultAsync(m => m.Id == adminQuickEditDto.Id);
            if (model is null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (adminQuickEditDto.AllowMultiLogin.IsNotNullOrEmpty())
                model.AllowMultiLogin = adminQuickEditDto.AllowMultiLogin.CastTo<bool>();
            if (adminQuickEditDto.Islock.IsNotNullOrEmpty())
                model.Islock = adminQuickEditDto.Islock.CastTo<bool>();
            try
            {
                UpdateLongEntityBasicInfo(model);
                adminRepository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
    }
}
