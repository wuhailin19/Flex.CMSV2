using AutoMapper.Configuration.Conventions;
using Flex.Core.Reflection;
using Flex.Domain.Dtos.RoleUrl;
using Flex.EFSqlServer.Repositories;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System.Linq.Expressions;
using System.Security;

namespace Flex.Application.Services
{
    public class RoleUrlServices : BaseService, IRoleUrlServices
    {
        private IEfCoreRespository<SysRoleUrl> repository;
        private IRoleServices _roleServices;
        public RoleUrlServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, IRoleServices roleServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            repository = unitOfWork.GetRepository<SysRoleUrl>();
            _roleServices = roleServices;
        }
        public async Task<ApiPermissionDto> GetRoleUrlListById(int Id)
        {
            var role = await _roleServices.GetRoleByIdAsync(Id);
            var jObj = role.UrlPermission.IsNullOrEmpty() ? new ApiPermissionDto() : JsonConvert.DeserializeObject<ApiPermissionDto>(role.UrlPermission ?? string.Empty);
            if (jObj != null)
            {
                return jObj;
            }
            return default;
        }
        public async Task<IEnumerable<RoleUrlListDto>> GetApiUrlListByCateId(string cateid, string k = null)
        {
            int cid = 0;
            if (cateid.IsNullOrEmpty())
                cid = 1;
            else
                cid = cateid.ToInt();
            if (_claims.IsSystem)
            {
                Expression<Func<SysRoleUrl, bool>> expression = m => m.Category == cateid.ToInt();
                if (k != null)
                    expression = expression.And(m => m.Url.Contains(k));
                var lists = await repository.GetAllAsync(expression);
                var models = _mapper.Map<List<RoleUrlListDto>>(lists);
                models.Add(new RoleUrlListDto { Id = "00000", ParentId = "000001", Url = "", Name = "快捷选择" });
                return models.OrderBy(m => m.Url);
            }
            else
            {
                var role = await _roleServices.GetCurrentRoldDtoAsync();

                var datamission = role.UrlPermission;
                var jObj = JsonConvert.DeserializeObject<ApiPermissionDto>(datamission);
                var model = new List<SysRoleUrl>();
                if (cid == 1)
                    model = (await repository.GetAllAsync(m => jObj.dataapi.ToList("-").Contains(m.Id))).ToList();
                else
                    model = (await repository.GetAllAsync(m => jObj.pageapi.ToList("-").Contains(m.Id))).ToList();
                Func<SysRoleUrl, bool> expression = m => m.Category == cateid.ToInt();
                if (k != null)
                    expression = m => m.Category == cateid.ToInt() && m.Url.Contains(k);
                var models = _mapper.Map<List<RoleUrlListDto>>(model);
                models.Add(new RoleUrlListDto { Id = "00000", ParentId = "000001", Url = "", Name = "快捷选择" });
                return models.OrderBy(m => m.Url);
            }
        }
        public async Task<ProblemDetails<string>> CreateUrlList()
        {
            List<ReflectMenuModel> data = ReflectionUrl.GetALLMenuByReflection();
            var menuapis = new List<SysRoleUrl>();
            DateTime dateTime = DateTime.Now;
            foreach (var item in data)
            {
                SysRoleUrl parentApi = new SysRoleUrl();
                parentApi.Id = EncryptHelper.MD5(item.MenuLink);
                parentApi.Url = item.MenuLink.ToLower().TrimEnd('/');
                parentApi.Name = item.MenuName;
                parentApi.Description = item.MenuName;
                parentApi.MaxErrorCount = 10;
                parentApi.ReturnContent = "";
                parentApi.NeedActionPermission = false;
                parentApi.RequestType = null;
                parentApi.OrderId = 1;
                parentApi.ParentId = "-1";
                parentApi.AddUserName = "system";
                parentApi.AddTime = Clock.Now;
                parentApi.LastEditDate = Clock.Now;
                parentApi.LastEditUserName = "system";

                foreach (var items in item.ActionList)
                {
                    SysRoleUrl menuApi = new SysRoleUrl();
                    menuApi.Id = EncryptHelper.MD5(item.MenuLink + items.ActionCode);
                    menuApi.Url = (item.MenuLink + items.ActionCode).ToLower();
                    menuApi.Name = items.ActionName;
                    menuApi.Description = items.ActionDesc;
                    menuApi.MaxErrorCount = 10;
                    menuApi.ReturnContent = "";
                    menuApi.NeedActionPermission = items.ActionPermission;
                    menuApi.RequestType = items.ActionType;
                    menuApi.Category = items.Cate;
                    if (parentApi.Category == 0)
                        parentApi.Category = items.Cate;
                    menuApi.OrderId = 1;
                    menuApi.ParentId = parentApi.Id;
                    menuApi.AddUserName = "system";
                    menuApi.AddTime = Clock.Now;
                    menuApi.LastEditDate = Clock.Now;
                    menuApi.LastEditUserName = "system";
                    if (!menuapis.Any(m => m.Id == menuApi.Id))
                        menuapis.Add(menuApi);
                }
                if (!menuapis.Any(m => m.Id == parentApi.Id))
                    menuapis.Add(parentApi);
            }
            try
            {
                if (Consts.Mode == ProductMode.Dev)
                {
                    _unitOfWork.ExecuteSqlCommand("delete from tbl_core_roleurl");
                    await _unitOfWork.GetRepository<SysRoleUrl>().InsertAsync(menuapis);
                    await _unitOfWork.SaveChangesAsync();
                }
                return new ProblemDetails<string>(HttpStatusCode.OK, "共添加" + menuapis.Count + "条数据");
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
