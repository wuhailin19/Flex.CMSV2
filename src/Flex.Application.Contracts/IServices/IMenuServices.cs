namespace Flex.Application.Contracts.IServices
{
    public interface IMenuServices
    {
        Task<IEnumerable<MenuDto>> GetMainMenuDtoAsync();
        Task<IEnumerable<MenuColumnDto>> GetMenuListAsync();
        Task<IEnumerable<MenuColumnDto>> getMenuShortcutAsync(string mode);
        Task<IEnumerable<MenuDto>> GetTreeMenuDtoByRoleIdAsync(long Id);
        Task<IEnumerable<MenuColumnDto>> TestGateWaygetMenuShortcutAsync(string mode);
    }
}
