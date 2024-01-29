using Consul;
using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.RoleUrl;
using Newtonsoft.Json;
using Org.BouncyCastle.Tls;
using ShardingCore.Extensions;
using System.Data;
using System.Text.RegularExpressions;

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

        #region 获取单角色
        /// <summary>
        /// 获取当前角色实体
        /// </summary>
        /// <returns></returns>
        public async Task<SysRole> GetCurrentRoldDtoAsync()
        {
            var currentrole = await _unitOfWork.GetRepository<SysRole>()
                        .GetFirstOrDefaultAsync(m => _claims.UserRole == m.Id.ToString(), null, null, true, false);
            if (currentrole is null)
                return default(SysRole);
            return currentrole;
        }
        /// <summary>
        /// 获取角色实体ById
        /// </summary>
        /// <returns></returns>
        public async Task<SysRole> GetRoleByIdAsync(string Id)
        {
            var currentrole = await _unitOfWork.GetRepository<SysRole>()
                        .GetFirstOrDefaultAsync(m => Id.Contains(m.Id.ToString()), null, null, true, false);
            if (currentrole is null)
                return default(SysRole);
            return currentrole;
        }
        #endregion

        /// <summary>
        /// 传入roleId获取栏目权限列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DataPermissionDto>> GetDataPermissionListById(int Id)
        {
            List<DataPermissionDto> permissionDtos = new List<DataPermissionDto>();
            var role = await _unitOfWork
                .GetRepository<SysRole>()
                .GetAllAsync(m => m.Id == Id);
            if (role.Count == 0)
                return permissionDtos;
            var model = role.FirstOrDefault();
            if (model.DataPermission.IsEmpty())
                return permissionDtos;
            permissionDtos = JsonConvert.DeserializeObject<List<DataPermissionDto>>(model.DataPermission).ToList();
            return permissionDtos;
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
                UpdateIntEntityBasicInfo(model);
                _unitOfWork.GetRepository<SysRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, "修改成功");
            }
            catch
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "修改失败");
            }
        }
        public async Task<ProblemDetails<string>> UpdateDataPermission(InputRoleDatapermissionDto role)
        {
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.Id == role.Id);
            if (model == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "该角色不存在");
            try
            {
                model.DataPermission = role.chooseId;
                UpdateIntEntityBasicInfo(model);
                _unitOfWork.GetRepository<SysRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, "修改成功");
            }
            catch
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "修改失败");
            }
        }
        public async Task<ProblemDetails<string>> UpdateApiPermission(InputRoleUrlDto role)
        {
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.Id == role.Id);
            if (model == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "该角色不存在");
            try
            {
                model.UrlPermission = role.sysapis;
                UpdateIntEntityBasicInfo(model);
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
            if (delete_list.Count == 0)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
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
        private const string pattern = "{[a-zA-Z]+}";
        public async Task<Dictionary<string, List<string>>> CurrentPermissionDtosAsync()
        {
            var result = new Dictionary<string, List<string>>();
            var RoleList = await GetRoleByRoleIdAsync(_claims.UserRole);
            var UrlList = await _unitOfWork.GetRepository<SysRoleUrl>().GetAllAsync();
            //var permisslists = new List<PermissionDto>();
            foreach (var item in RoleList)
            {
                if (item.UrlPermission.IsNotNullOrEmpty())
                {
                    var apipermissionmodel = JsonConvert.DeserializeObject<ApiPermissionDto>(item.UrlPermission) ;
                    var dataapi_list = UrlList.Where(u => apipermissionmodel.dataapi.Split('-').Contains(u.Id.ToString()));
                    var pageapi_list = UrlList.Where(u => apipermissionmodel.pageapi.Split('-').Contains(u.Id.ToString()));
                    List<string> strings = new List<string>();
                    foreach (var dataapi in dataapi_list)
                    {
                        strings.Add(Regex.Replace(dataapi.Url, pattern, ""));
                    }
                    foreach (var pageapi in pageapi_list)
                    {
                        strings.Add(Regex.Replace(pageapi.Url, pattern, ""));
                    }
                    result.Add(item.Id.ToString(), strings);
                }
            }
            return result;
        }
    }
}
