using Flex.Domain.Dtos.IndexShortCut;
using Flex.Domain.Entities;

namespace Flex.Application.Contracts.IServices
{
    public interface ISystemIndexSetServices
    {
        Task<int> AddAsync(SysIndexSet sysIndexSet);
        /// <summary>
        /// 删除当前快捷
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task<int> DeleteCurrentAsync(ShortCutDtos shortCutDtos);
        /// <summary>
        /// 获取当前登录账户数据
        /// </summary>
        /// <param name="sysIndexSet"></param>
        /// <returns></returns>
        Task<SysIndexSet> GetbyCurrentIdAsync();
        /// <summary>
        /// 根据管理员Id获取快捷方式
        /// </summary>
        /// <returns></returns>
        Task<SysIndexSet> GetSysIndexSetbyAdminIdAsync(long UserId);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysIndexSet"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(SysIndexSet sysIndexSet);
        /// <summary>
        /// 修改当前快捷
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task<int> UpdateCurrentAsync(ShortCutDtos shortCutDtos);
    }
}
