using Microsoft.AspNetCore.Mvc;

namespace Flex.Application.Services
{
    public class MenuServices : BaseService, IMenuServices
    {


        private ISystemIndexSetServices _systemIndexSetServices;
        public MenuServices(IUnitOfWork unitOfWork, ISystemIndexSetServices systemIndexSetServices, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims) :
            base(unitOfWork, mapper, idWorker, claims)
        {
            _systemIndexSetServices = systemIndexSetServices;
        }
        private List<MenuDto> Main = new List<MenuDto>();
        private List<SysMenu> treelist = new List<SysMenu>(); //菜单编辑数据树、左侧菜单树

        private void AddMenu(List<MenuDto> all, MenuDto curItem)
        {
            List<MenuDto> childItems = all.Where(ee => ee.parentid == curItem.id).ToList(); //得到子节点
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
        public async Task<IEnumerable<MenuDto>> GetTreeMenuAsync() {
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
                var currentrole = await GetCurrentRoldDtoAsync();
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
        public async Task<IEnumerable<MenuColumnDto>?> getMenuShortcutAsync(string mode)
        {
            IEnumerable<SysMenu> list = await GetAllMenuList();
            IEnumerable<SysMenu> menu_list = null;
            //超管直接返回所有菜单
            if (_claims.IsSystem)
            {
                menu_list = list.Where(m => m.isMenu == false && m.ShowStatus == true);
            }
            else
            {
                var currentrole = await GetCurrentRoldDtoAsync();
                if (currentrole is null)
                    return default(IEnumerable<MenuColumnDto>);
                menu_list = list.Where(m => m.isMenu == false && currentrole.MenuPermissions.ToList().Contains(m.Id.ToString()) && m.ShowStatus == true);
            }
            if (menu_list.IsNullOrEmpty())
                return default(IEnumerable<MenuColumnDto>);

            var systemindexset = await _systemIndexSetServices.GetbyCurrentIdAsync();
            switch (mode)
            {
                case "1":
                    if (systemindexset.SystemMenu.IsNullOrEmpty())
                        return default(IEnumerable<MenuColumnDto>);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.SystemMenu.ToList().Contains(m.Id.ToString())));
                case "3":
                    if (systemindexset.SystemMenu.IsNullOrEmpty())
                        return _mapper.Map<List<MenuColumnDto>>(menu_list);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.SystemMenu.ToList().Contains(m.Id.ToString()) == false));
                case "2":
                    if (systemindexset.SiteMenu.IsNullOrEmpty())
                        return default(IEnumerable<MenuColumnDto>);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.SiteMenu.ToList().Contains(m.Id.ToString())));
                case "4":
                    if (systemindexset.SiteMenu.IsNullOrEmpty())
                        return _mapper.Map<List<MenuColumnDto>>(menu_list);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.SiteMenu.ToList().Contains(m.Id.ToString()) == false));
                case "7":
                    if (systemindexset.FileManage.IsNullOrEmpty())
                        return default(IEnumerable<MenuColumnDto>);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.FileManage.ToList().Contains(m.Id.ToString())));
                case "8":
                    if (systemindexset.FileManage.IsNullOrEmpty())
                        return _mapper.Map<List<MenuColumnDto>>(menu_list);
                    return _mapper.Map<List<MenuColumnDto>>(menu_list.Where(m => systemindexset.FileManage.ToList().Contains(m.Id.ToString()) == false));
            }
            return default(IEnumerable<MenuColumnDto>);
        }
        /// <summary>
        /// 获取当前角色菜单树
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MenuDto>?> GetCurrentMenuDtoByRoleIdAsync()
        {
            IEnumerable<SysMenu> list =  await _unitOfWork.GetRepository<SysMenu>().GetAllAsync();
            //超管直接返回所有菜单
            if (_claims.IsSystem)
            {
            }
            else
            {
                var currentrole = await GetCurrentRoldDtoAsync();
                if (currentrole is null)
                    return default;
                var children = list.Where(m => currentrole.MenuPermissions.Split(',').Contains(m.Id.ToString()));
                treelist.Add(list.Where(x => x.ParentID == 0).FirstOrDefault());
                AddMenuTreeTable(list, children);
                list = treelist;
            }
            var model = await GetRoleByIdAsync(_claims.UserId);
            string[] menus = null;
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.MenuPermissions))
                    menus = model.MenuPermissions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                else
                    menus = new string[] { };
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
        public async Task<SysRole> GetRoleByIdAsync(long Id)
        {
            var currentrole = await _unitOfWork.GetRepository<SysRole>()
                        .GetFirstOrDefaultAsync(m => Id == m.Id, null, null, true, false);
            if (currentrole is null)
                return default(SysRole);
            return currentrole;
        }
        /// <summary>
        /// 获取主界面菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MenuDto>> GetMainMenuDtoAsync()
        {
            IEnumerable<SysMenu>  list = await _unitOfWork.GetRepository<SysMenu>().GetAllAsync();
        
            //超管直接返回所有菜单
            if (_claims.IsSystem)
            {
            }
            else
            {
                var currentrole = await GetCurrentRoldDtoAsync();
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
            var menumodel =await menuRepository.GetFirstOrDefaultAsync(m=>m.Id== model.Id);
            menumodel.isMenu= model.isMenu;
            menumodel.FontSort=model.FontSort;
            menumodel.Icode=model.Icode;
            menumodel.IsControllerUrl = model.IsControllerUrl;
            menumodel.LinkUrl = model.LinkUrl;
            menumodel.Name = model.Name;
            menumodel.OrderId = model.OrderId;
            menumodel.ParentID = model.ParentID;
            menumodel.isMenu = model.isMenu;
            menumodel.LastEditUser = _claims.UserId;
            menumodel.LastEditUserName = _claims.UserName;
            menumodel.LastEditDate = Clock.Now;
            menumodel.Version += 1;
            try
            {
                menuRepository.Update(menumodel);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, "修改成功");
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "修改失败");
            }
        }
    }
}
