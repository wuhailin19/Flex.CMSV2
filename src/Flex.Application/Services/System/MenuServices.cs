using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.System.Menu;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Flex.Application.Services
{
    public class MenuServices : BaseService, IMenuServices
    {

        private IRoleServices _roleServices;
        private ISystemIndexSetServices _systemIndexSetServices;
        public MenuServices(IUnitOfWork unitOfWork, ISystemIndexSetServices systemIndexSetServices, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, IRoleServices roleServices) :
            base(unitOfWork, mapper, idWorker, claims)
        {
            _systemIndexSetServices = systemIndexSetServices;
            _roleServices = roleServices;
        }
        private List<MenuDto> Main = new List<MenuDto>();
        private List<SysMenu> treelist = new List<SysMenu>(); //菜单编辑数据树、左侧菜单树

        private void AddMenu(List<MenuDto> all, MenuDto curItem)
        {
            List<MenuDto> childItems = all.Where(ee => ee.parentid == curItem.id).OrderBy(x=>x.OrderId).ThenBy(x=>x.id).ToList(); //得到子节点
            curItem.children = childItems; //将子节点加入
                                           //遍历子节点，进行递归，寻找子节点的子节点
            foreach (var subItem in childItems)
            {
                AddMenu(all, subItem);
            }
        }
        private void AddMenuTreeTable(IEnumerable<SysMenu> lists, IEnumerable<SysMenu> treeTables)
        {
            List<SysMenu> childrens = null;
            foreach (var item in treeTables)
            {
                if (item.ParentID != 0)
                {
                    childrens = lists.Where(m => m.Id == item.ParentID).ToList();
                    if (item != null && !treelist.Contains(item))
                    {
                        treelist.Add(item);
                    }
                    AddMenuTreeTable(lists, childrens);
                }
            }
        }
        public async Task<IEnumerable<MenuDto>> GetTreeMenuAsync()
        {
            IEnumerable<SysMenu> list = await _unitOfWork.GetRepository<SysMenu>().GetAllAsync();
            var query = _mapper.Map<List<MenuDto>>(list.OrderBy(m => m.OrderId));
            return query;
        }
        /// <summary>
        /// 获取菜单管理页列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MenuColumnDto>> GetMenuListAsync()
        {
            IEnumerable<SysMenu> list = await _unitOfWork.GetRepository<SysMenu>().GetAllAsync();
            //超管直接返回所有菜单
            if (_claims.IsSystem)
            {
            }
            else
            {
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                if (currentrole is null)
                    return default;
                var children = list.Where(m => currentrole.MenuPermissions.Split(',').Contains(m.Id.ToString()));
                treelist.Add(list.Where(x => x.ParentID == 0).FirstOrDefault());
                AddMenuTreeTable(list, children);
                list = treelist;
            }
            if (list.IsNullOrEmpty())
                return default;
            return _mapper.Map<IEnumerable<MenuColumnDto>>(list);
        }
        private async Task<IEnumerable<SysMenu>> GetAllMenuList()
        {
            var list = await _unitOfWork.GetRepository<SysMenu>().GetAllAsync();
            return list;
        }


        /// <summary>
        /// 获取快捷菜单
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MenuColumnDto>> getMenuShortcutAsync(string mode)
        {
            IEnumerable<SysMenu> list = await GetAllMenuList();
            IEnumerable<SysMenu> menu_list = new List<SysMenu>();
            //超管直接返回所有菜单
            if (_claims.IsSystem)
            {
                menu_list = list.Where(m => m.isMenu == false && m.ShowStatus == true);
            }
            else
            {
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                if (currentrole is null)
                    return default;
                menu_list = list.Where(m => m.isMenu == false && currentrole.MenuPermissions.ToList().Contains(m.Id.ToString()) && m.ShowStatus == true);
            }
            if (menu_list.IsNullOrEmpty())
                return default;

            var systemindexset = await _systemIndexSetServices.GetbyCurrentIdAsync();
            switch (mode)
            {
                case "1":
                    if (systemindexset.SystemMenu.IsNullOrEmpty())
                        return default;
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.SystemMenu.ToList().Contains(m.Id.ToString())));
                case "3":
                    if (systemindexset.SystemMenu.IsNullOrEmpty())
                        return _mapper.Map<List<MenuColumnDto>>(menu_list);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.SystemMenu.ToList().Contains(m.Id.ToString()) == false));
                case "2":
                    if (systemindexset.SiteMenu.IsNullOrEmpty())
                        return default;
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.SiteMenu.ToList().Contains(m.Id.ToString())));
                case "4":
                    if (systemindexset.SiteMenu.IsNullOrEmpty())
                        return _mapper.Map<List<MenuColumnDto>>(menu_list);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.SiteMenu.ToList().Contains(m.Id.ToString()) == false));
                case "7":
                    if (systemindexset.FileManage.IsNullOrEmpty())
                        return default;
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.FileManage.ToList().Contains(m.Id.ToString())));
                case "8":
                    if (systemindexset.FileManage.IsNullOrEmpty())
                        return _mapper.Map<List<MenuColumnDto>>(menu_list);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.FileManage.ToList().Contains(m.Id.ToString()) == false));
            }
            return default;
        }
        /// <summary>
        /// 获取当前角色菜单树
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MenuDto>> GetCurrentMenuDtoByRoleIdAsync()
        {
            IEnumerable<SysMenu> list = await _unitOfWork.GetRepository<SysMenu>().GetAllAsync();
            string[] menus = null;
            //超管直接返回所有菜单
            if (_claims.IsSystem)
            {
            }
            else
            {
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                if (currentrole is null)
                    return default;
                if (!string.IsNullOrEmpty(currentrole.MenuPermissions))
                    menus = currentrole.MenuPermissions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                else
                    menus = new string[] { };
                var children = list.Where(m => currentrole.MenuPermissions.Split(',').Contains(m.Id.ToString()));
                treelist.Add(list.Where(x => x.ParentID == 0).FirstOrDefault());
                AddMenuTreeTable(list, children);
                list = treelist;
            }
            if (list.IsNullOrEmpty())
                return default;
            var query = _mapper.Map<List<MenuDto>>(list.OrderBy(m => m.OrderId));
            query.Each(item =>
            {
                item.@checked =
                        menus != null ?
                        menus.Contains(item.id.ToString()) &&
                        item.parentid != 0 ? true : false : false;
            });
            Main.Add(query.Where(m => m.parentid == 0).FirstOrDefault());//根节点
            AddMenu(query, query.Where(x => x.parentid == 0).FirstOrDefault());
            return Main;
        }
        /// <summary>
        /// 获取角色菜单树
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MenuDto>> GetMenuDtoByRoleIdAsync(int Id)
        {
            IEnumerable<SysMenu> list = await _unitOfWork.GetRepository<SysMenu>().GetAllAsync();
            string[] menus = null;

            //超管直接返回所有菜单
            if (_claims.IsSystem)
            {
            }
            else
            {
                var loginrole = await _roleServices.GetRoleByIdAsync(_claims.UserRole);
                if (loginrole is null)
                    return default;
                var children = list.Where(m => loginrole.MenuPermissions.Split(',').Contains(m.Id.ToString()));
                treelist.Add(list.Where(x => x.ParentID == 0).FirstOrDefault());
                AddMenuTreeTable(list, children);
                list = treelist;
            }
            if (list.IsNullOrEmpty())
                return default;
            var currentrole = await _roleServices.GetRoleByIdAsync(Id);
            if (!string.IsNullOrEmpty(currentrole.MenuPermissions))
                menus = currentrole.MenuPermissions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            else
                menus = new string[] { };
            var query = _mapper.Map<List<MenuDto>>(list.OrderBy(m => m.OrderId));
            query.Each(item =>
            {
                item.@checked =
                        menus != null ?
                        menus.Contains(item.id.ToString()) &&
                        item.parentid != 0 ? true : false : false;
            });
            Main.Add(query.Where(m => m.parentid == 0).FirstOrDefault());//根节点
            AddMenu(query, query.Where(x => x.parentid == 0).FirstOrDefault());
            return Main;
        }

        /// <summary>
        /// 获取主界面菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MenuDto>> GetMainMenuDtoAsync()
        {

            Func<IQueryable<SysMenu>, IOrderedQueryable<SysMenu>> orderby = m => m.OrderBy(x => x.OrderId).ThenBy(x => x.Id);
            var list = (await _unitOfWork.GetRepository<SysMenu>().GetAllAsync(null, orderby)).ToList();

            //超管直接返回所有菜单
            if (_claims.IsSystem)
            {
            }
            else
            {
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                if (currentrole is null)
                    return default;
                var children = list.Where(m => currentrole.MenuPermissions.Split(',').Contains(m.Id.ToString()));
                treelist.Add(list.Where(x => x.ParentID == 0).FirstOrDefault());
                AddMenuTreeTable(list, children);
                list = treelist;
            }
            if (list.IsNullOrEmpty())
                return default(IEnumerable<MenuDto>);
            var query = _mapper.Map<List<MenuDto>>(list);
            Main.Add(query.Where(m => m.parentid == 0).FirstOrDefault());//根节点
            AddMenu(query, query.Where(x => x.parentid == 0).FirstOrDefault());

            return Main;
        }

        public async Task<ProblemDetails<string>> EditMenu(MenuEditDto model)
        {
            var menuRepository = _unitOfWork.GetRepository<SysMenu>();
            var menumodel = await menuRepository.GetFirstOrDefaultAsync(m => m.Id == model.Id);
            menumodel.isMenu = model.isMenu;
            menumodel.FontSort = model.FontSort;
            menumodel.Icode = model.Icode;
            menumodel.IsControllerUrl = model.IsControllerUrl;
            menumodel.LinkUrl = model.LinkUrl;
            menumodel.Name = model.Name;
            menumodel.OrderId = model.OrderId;
            menumodel.ParentID = model.ParentID;
            menumodel.ShowStatus = model.Status;
            UpdateIntEntityBasicInfo(menumodel);
            try
            {
                menuRepository.Update(menumodel);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(),ex);
            }
        }
        public async Task<ProblemDetails<string>> QuickEditMenu(MenuQuickEditDto menuQuickEditDto)
        {
            var adminRepository = _unitOfWork.GetRepository<SysMenu>();
            var model = await adminRepository.GetFirstOrDefaultAsync(m => m.Id == menuQuickEditDto.Id);
            if (model is null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (menuQuickEditDto.isMenu.IsNotNullOrEmpty())
                model.isMenu = menuQuickEditDto.isMenu.CastTo<bool>();
            if (menuQuickEditDto.Status.IsNotNullOrEmpty())
                model.ShowStatus = menuQuickEditDto.Status.CastTo<bool>();
            try
            {
                UpdateIntEntityBasicInfo(model);
                adminRepository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<string>> AddMenu(MenuAddDto model)
        {
            var menuRepository = _unitOfWork.GetRepository<SysMenu>();
            var menumodel = _mapper.Map<SysMenu>(model);
            AddIntEntityBasicInfo(menumodel);
            try
            {
                menuRepository.Insert(menumodel);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataInsertError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<SysMenu>();
            if (Id.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<SysMenu>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                }
                adminRepository.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch(Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(), ex);
            }
        }
    }
}
