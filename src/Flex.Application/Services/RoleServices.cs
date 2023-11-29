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
        public async Task<IEnumerable<RoleDto>> GetCurrentAdminRoleByTokenAsync() {
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
            PagedList<RoleColumnDto> trees =_mapper
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
