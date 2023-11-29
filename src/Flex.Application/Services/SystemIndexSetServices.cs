using Flex.Domain.Dtos.IndexShortCut;

namespace Flex.Application.Services
{
    public class SystemIndexSetServices : BaseService, ISystemIndexSetServices
    {
        public SystemIndexSetServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sysIndexSet"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(SysIndexSet sysIndexSet)
        {
            await _unitOfWork.GetRepository<SysIndexSet>().InsertAsync(sysIndexSet);
            return await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysIndexSet"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(SysIndexSet sysIndexSet)
        {
            _unitOfWork.GetRepository<SysIndexSet>().Update(sysIndexSet);
            return await _unitOfWork.SaveChangesAsync();
        }
        
        public async Task<int> UpdateCurrentAsync(ShortCutDtos shortCutDtos)
        {
            var model = await GetbyCurrentIdAsync();
            switch (shortCutDtos.mode)
            {
                case "3":
                    if (!model.SystemMenu.IsNullOrEmpty())
                        shortCutDtos.Menu = "," + shortCutDtos.Menu;
                    model.SystemMenu = model.SystemMenu + shortCutDtos.Menu;
                    break;
                case "4":
                    if (!model.SiteMenu.IsNullOrEmpty())
                        shortCutDtos.Menu = "," + shortCutDtos.Menu;
                    model.SiteMenu = model.SiteMenu + shortCutDtos.Menu;
                    break;
                case "6":
                    if (!model.Shortcut.IsNullOrEmpty())
                        shortCutDtos.Menu = "," + shortCutDtos.Menu;
                    model.Shortcut = model.Shortcut + shortCutDtos.Menu;
                    break;
                case "8":
                    if (!model.FileManage.IsNullOrEmpty())
                        shortCutDtos.Menu = "," + shortCutDtos.Menu;
                    model.FileManage = model.FileManage + shortCutDtos.Menu;
                    break;
            }
            return (await UpdateAsync(model));
        }
        
        public async Task<int> DeleteCurrentAsync(ShortCutDtos shortCutDtos)
        {
            var model = await GetbyCurrentIdAsync();
            switch (shortCutDtos.mode)
            {
                case "3":
                    model.SystemMenu = shortCutDtos.Menu;
                    break;
                case "4":
                    model.SiteMenu = shortCutDtos.Menu;
                    break;
                case "6":
                    model.Shortcut = shortCutDtos.Menu;
                    break;
                case "8":
                    model.FileManage = shortCutDtos.Menu;
                    break;
            }
            return (await UpdateAsync(model));
        }
        /// <summary>
        /// 根据管理员Id获取快捷方式
        /// </summary>
        /// <returns></returns>
        public async Task<SysIndexSet> GetSysIndexSetbyAdminIdAsync(long UserId) {
            var result = await _unitOfWork.GetRepository<SysIndexSet>().GetAllAsync(m => m.AdminId == UserId);
            if (result.IsNullOrEmpty())
            {
                var model = new SysIndexSet();
                model.AdminId = UserId;
                await AddAsync(model);
                return model;
            }
            return result.FirstOrDefault();
        }
        /// <summary>
        /// 获取当前登录账户数据
        /// </summary>
        /// <param name="sysIndexSet"></param>
        /// <returns></returns>
        public async Task<SysIndexSet> GetbyCurrentIdAsync()
        {
            return await GetSysIndexSetbyAdminIdAsync(_claims.UserId);
        }
    }
}
