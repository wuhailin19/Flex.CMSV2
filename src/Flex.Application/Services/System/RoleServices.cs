using Consul;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.ServicesHelper;
using Flex.Domain.Cache;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.RoleUrl;
using Flex.Domain.Entities.System;
using Newtonsoft.Json;
using Org.BouncyCastle.Tls;
using ShardingCore.Extensions;
using System.Data;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace Flex.Application.Services
{
    public class RoleServices : BaseService, IRoleServices
    {
        ICaching _caching;
        public RoleServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, ICaching caching)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _caching = caching;
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
        public async Task<IEnumerable<SysRole>> GetRoleByRoleIdAsync(int roleId)
        {
            return await _unitOfWork
                .GetRepository<SysRole>()
                .GetAllAsync(m => m.Id == roleId);
        }

        #region 获取单角色
        /// <summary>
        /// 获取当前角色实体
        /// </summary>
        /// <returns></returns>
        public async Task<SysRole> GetCurrentRoldDtoAsync()
        {
            var rolekey = RoleKeys.RoleCache + _claims.UserRole;
            if (_caching.Exist(rolekey))
                return _caching.Get(rolekey) as SysRole;
            var currentrole = await _unitOfWork.GetRepository<SysRole>()
                        .GetFirstOrDefaultAsync(m => _claims.UserRole == m.Id, null, null, true, false);
            _caching.Set(rolekey, currentrole, 60);
            if (currentrole is null)
                return default(SysRole);
            return currentrole;
        }
        /// <summary>
        /// 获取角色实体ById
        /// </summary>
        /// <returns></returns>
        public async Task<SysRole> GetRoleByIdAsync(int Id)
        {
            var currentrole = await _unitOfWork.GetRepository<SysRole>()
                        .GetFirstOrDefaultAsync(m => m.Id == Id);
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
        public async Task<DataPermissionDto> GetDataPermissionListById(int Id,int siteId)
        {
            var role = await _unitOfWork
                .GetRepository<SysRole>()
                .GetAllAsync(m => m.Id == Id);
            if (role.Count == 0)
                return default;
            var model = role.FirstOrDefault();
            if (model.DataPermission.IsEmpty())
                return default;
            DataPermissionDto permissionDtos = GetSitePermissionDto(model.DataPermission, siteId);
            return permissionDtos;
        }

        /// <summary>
        /// 传入roleId获取站点权限
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<string> GetSitePermissionListById(int Id)
        {
            var role = await _unitOfWork
                .GetRepository<SysRole>()
                .GetAllAsync(m => m.Id == Id);
            if (role.Count == 0)
                return default;
            var model = role.FirstOrDefault();
            if (model.WebsitePermissions.IsEmpty())
                return default;
            return model.WebsitePermissions;
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

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RoleStepDto>> GetStepRoleListAsync()
        {
            var rolelist = await _unitOfWork
                .GetRepository<SysRole>()
                .GetAllAsync();
            var dtolist = _mapper.Map<IEnumerable<RoleStepDto>>(rolelist).ToList();
            dtolist.Add(new RoleStepDto
            {
                Id = "00000",
                RolesName = "快速选择"
            });
            return dtolist.OrderBy(m => m.Id);
        }

        public async Task<ProblemDetails<string>> AddNewRole(InputRoleDto role)
        {
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.RolesName == role.RolesName);
            if (model != null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataExist.GetEnumDescription());
            var insertmodel = _mapper.Map<SysRole>(role);
            AddIntEntityBasicInfo(insertmodel);
            var result = await _unitOfWork.GetRepository<SysRole>().InsertAsync(insertmodel);
            await _unitOfWork.SaveChangesAsync();
            if (result.Entity.Id > 0)
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
        }
        public async Task<ProblemDetails<string>> UpdateRole(InputUpdateRoleDto role)
        {
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.RolesName == role.RolesName);
            if (model != null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataExist.GetEnumDescription());
            try
            {
                model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.Id == role.Id);
                _mapper.Map(role, model);
                UpdateIntEntityBasicInfo(model);
                _unitOfWork.GetRepository<SysRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                _caching.Remove(RoleKeys.RoleCache + model.Id);
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> UpdateMenuPermission(InputRoleMenuDto role)
        {
            var checkres = await CheckCurrentRoleIsUltravires("menu", role.MenuPermissions);
            if (!checkres)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.Id == role.Id);
            if (model == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            try
            {
                model.MenuPermissions = role.MenuPermissions;
                UpdateIntEntityBasicInfo(model);
                _unitOfWork.GetRepository<SysRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                _caching.Remove(RoleKeys.RoleCache + model.Id);
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
        /// <summary>
        /// 判断是否越权进行授权操作
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckCurrentRoleIsUltravires(string cate, string rolestr)
        {
            if (_claims.IsSystem)
                return true;
            var role = await GetCurrentRoldDtoAsync();
            switch (cate)
            {
                case "api":
                    var currentobject = JsonConvert.DeserializeObject<ApiPermissionDto>(role.UrlPermission);
                    var updateobject = JsonConvert.DeserializeObject<ApiPermissionDto>(rolestr);
                    var currentroleidlist = currentobject.dataapi.ToList("-");
                    if (currentroleidlist.Count == 0)
                        return false;
                    var updateidlist = updateobject.dataapi.ToList("-");
                    foreach (var item in updateidlist)
                    {
                        if (!currentroleidlist.Contains(item))
                            return false;
                    }
                    return true;
                case "menu":
                    if (role.MenuPermissions.IsNullOrEmpty())
                        return false;
                    var currentmenulist = role.MenuPermissions.ToList(",");
                    var updatemenulist = rolestr.ToList(",");
                    if (currentmenulist.Count == 0)
                        return false;
                    foreach (var item in updatemenulist)
                    {
                        if (!currentmenulist.Contains(item))
                            return false;
                    }
                    return true;
                case "datapermission":
                    var datapermission = JsonHelper.Json<DataPermissionDto>(rolestr);
                    var currentpermission = JsonHelper.Json<DataPermissionDto>(role.DataPermission);
                    return CheckColumnPermission.CheckPermission(currentpermission, datapermission);
                case "sitepermission":
                    var currentsiteper = role.WebsitePermissions;
                    if (currentsiteper.IsNullOrEmpty())
                        return false;
                    var currentsitelist = currentsiteper.ToList("-");
                    var updatesitelist = rolestr.ToList("-");
                    foreach (var item in updatesitelist)
                    {
                        if (!currentsitelist.Contains(item))
                            return false;
                    }
                    return true;
            }
            return false;
        }
        public async Task<ProblemDetails<string>> UpdateSitePermission(InputRoleSitepermissionDto role)
        {
            var checkres = await CheckCurrentRoleIsUltravires("sitepermission", role.chooseId);
            if (!checkres)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.Id == role.Id);
            if (model == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            try
            {
                model.WebsitePermissions = role.chooseId;
                UpdateIntEntityBasicInfo(model);
                _unitOfWork.GetRepository<SysRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                _caching.Remove(RoleKeys.RoleCache + model.Id);
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<string>> UpdateDataPermission(InputRoleDatapermissionDto role)
        {
            var checkres = await CheckCurrentRoleIsUltravires("datapermission", role.chooseId);
            if (!checkres)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.Id == role.Id);
            if (model == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            try
            {
                var datapermission = JsonHelper.Json<List<sitePermissionDto>>(model.DataPermission ?? string.Empty);
                if (datapermission == null)
                    datapermission = new List<sitePermissionDto>();

                var currentrolepermission = datapermission.Where(m => m.siteId == role.siteId).FirstOrDefault();
                if (currentrolepermission != null)
                    currentrolepermission.columnPermission = JsonHelper.Json<DataPermissionDto>(role.chooseId);
                else
                {
                    datapermission.Add(new sitePermissionDto() { siteId = role.siteId, columnPermission = JsonHelper.Json<DataPermissionDto>(role.chooseId) });
                }
                model.DataPermission = JsonHelper.ToJson(datapermission);

                UpdateIntEntityBasicInfo(model);
                _unitOfWork.GetRepository<SysRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                _caching.Remove(RoleKeys.userDataPermissionKey + model.Id);
                _caching.Remove(RoleKeys.RoleCache + model.Id);
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<string>> UpdateApiPermission(InputRoleUrlDto role)
        {
            var checkres = await CheckCurrentRoleIsUltravires("api", role.sysapis);
            if (!checkres)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var model = await _unitOfWork.GetRepository<SysRole>().GetFirstOrDefaultAsync(m => m.Id == role.Id);
            if (model == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            try
            {
                model.UrlPermission = role.sysapis;
                UpdateIntEntityBasicInfo(model);
                _unitOfWork.GetRepository<SysRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                string userid = RoleKeys.userRoleKey + model.Id;
                _caching.Remove(userid);
                _caching.Remove(RoleKeys.RoleCache + model.Id);
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<SysRole>();
            if (Id.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<SysRole>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    _caching.Remove(RoleKeys.userDataPermissionKey + item.Id);
                    _caching.Remove(RoleKeys.userRoleKey + item.Id);
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
        public async Task<Dictionary<string, List<string>>> PermissionDtosAsync()
        {
            var result = new Dictionary<string, List<string>>();
            var RoleList = (await _unitOfWork.GetRepository<SysRole>().GetAllAsync()).ToList();
            var UrlList = (await _unitOfWork.GetRepository<SysRoleUrl>().GetAllAsync()).ToList();
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
        private const string pattern = "{[a-zA-Z]+}/?";

        public async Task<Dictionary<int, List<string>>> CurrentUrlPermissionDtosAsync()
        {
            var result = new Dictionary<int, List<string>>();
            var currentRole = await GetCurrentRoldDtoAsync();
            var urlList = await _unitOfWork.GetRepository<SysRoleUrl>().GetAllAsync();

            if (currentRole?.UrlPermission.IsNullOrEmpty() ?? true)
                return result;

            var apiPermissionModel = JsonConvert.DeserializeObject<ApiPermissionDto>(currentRole.UrlPermission);
            var dataAndPageApiList = urlList
                .Where(u =>
                            apiPermissionModel.dataapi.Split('-').Contains(u.Id.ToString()) ||
                            apiPermissionModel.pageapi.Split('-').Contains(u.Id.ToString()))
                .OrderBy(m => m.Url)
                .ToList();

            var parentlist = dataAndPageApiList.Where(m => m.ParentId == "-1").ToList();
            List<string> strings = new List<string>();
            foreach (var item in parentlist)
            {
                dataAndPageApiList.RemoveAll(m => m.ParentId == item.Id);
            }

            var urls = dataAndPageApiList
                .Select(api => Regex.Replace(api.Url, pattern, ""))
                .ToList();

            result.Add(currentRole.Id, urls);
            return result;
        }
    }
}
