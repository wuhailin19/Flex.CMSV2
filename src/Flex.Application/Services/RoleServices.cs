using Consul;
using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.Role;

namespace Flex.Application.Services
{
    public class RoleServices : BaseService, IRoleServices
    {
        public RoleServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }

        /// <summary>
        /// 根据Token获取当前角色权限
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RoleDto>> GetCurrentAdminRoleByTokenAsync()
        {
            if (_claims is null)
                return default;
            return _mapper.Map<IEnumerable<RoleDto>>(await GetRoleByRoleIdAsync(_claims.UserRole));
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<PagedList<RoleColumnDto>> GetRoleListAsync(int page, int pagesize)
        {
            var list = await _unitOfWork.GetRepository<SysRole>().GetPagedListAsync(null, null, null, page, pagesize);
            PagedList<RoleColumnDto> trees = _mapper
                .Map<PagedList<RoleColumnDto>>(list);
            return trees;
        }
        /// <summary>
        /// 传入roleId获取角色列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysRole>> GetRoleByRoleIdAsync(string roleId)
        {
            var rolelist = roleId.ToList();
            return await _unitOfWork
                .GetRepository<SysRole>()
                .GetAllAsync(m => rolelist.Contains(m.Id.ToString()), null, null, true, false);
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RoleSelectDto>> GetRoleListAsync()
        {
            var rolelist = await _unitOfWork
                .GetRepository<SysRole>()
                .GetAllAsync();
            return _mapper.Map<IEnumerable<RoleSelectDto>>(rolelist);
        }

        public async Task<ProblemDetails<string>> AddNewRole(InputRoleDto role)
        {
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.RolesName == role.RolesName);
            if (model != null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "该角色已存在");
            var result = await _unitOfWork.GetRepository<SysRole>().InsertAsync(_mapper.Map<SysRole>(role));
            await _unitOfWork.SaveChangesAsync();
            if (result.Entity.Id > 0)
                return new ProblemDetails<string>(HttpStatusCode.OK, "添加成功");
            return new ProblemDetails<string>(HttpStatusCode.BadRequest, "添加失败");
        }

        public async Task<ProblemDetails<string>> UpdateMenuPermission(InputRoleMenuDto role)
        {
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.Id == role.Id);
            if (model == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "该角色不存在");
            try
            {
                model.MenuPermissions = role.MenuPermissions;
                model.LastEditUser = _claims.UserId;
                model.LastEditUserName= _claims.UserName;
                model.LastEditDate = Clock.Now;
                model.Version += 1;
                
                _unitOfWork.GetRepository<SysRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, "修改成功");
            }
            catch
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "修改失败");
            }
        }
        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<SysRole>();
            if (Id.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            try
            {
                var softdels = new List<SysRole>();
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
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            }
        }
        public async Task<Dictionary<string, List<string>>> PermissionDtosAsync()
        {

            var result = new Dictionary<string, List<string>>();
            var RoleList = await _unitOfWork.GetRepository<SysRole>().GetAllAsync();

            var UrlList = await _unitOfWork.GetRepository<SysRoleUrl>().GetAllAsync();

            //var permisslists = new List<PermissionDto>();

            RoleList.Foreach(item =>
            {
                if (!item.UrlPermission.IsNullOrEmpty())
                {
                    var u_list = UrlList.Where(u => item.UrlPermission.Split(',').Contains(u.Id.ToString()));

                    //var model = new PermissionDto();

                    result.Add(item.Id.ToString(),
                        (from u in u_list
                         select u.Url.ToLower())
                         .ToList());
                    //model.RoleId = item.Id;

                    //model.UrlList = (from u in u_list
                    //                 select u.Url.ToLower()).ToList();
                    //permisslists.Add(model);
                }
            });

            return result;
        }
    }
}
