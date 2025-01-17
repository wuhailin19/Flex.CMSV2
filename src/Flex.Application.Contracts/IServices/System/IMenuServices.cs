﻿using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.System.Menu;
using Flex.Domain.Entities;

namespace Flex.Application.Contracts.IServices
{
    public interface IMenuServices
    {
        Task<IEnumerable<MenuDto>> GetMainMenuDtoAsync();
        Task<IEnumerable<MenuColumnDto>> GetMenuListAsync();
        Task<IEnumerable<MenuColumnDto>> getMenuShortcutAsync(string mode);
        Task<IEnumerable<MenuDto>> GetCurrentMenuDtoByRoleIdAsync();
        Task<IEnumerable<MenuDto>> GetTreeMenuAsync();
        Task<ProblemDetails<string>> EditMenu(MenuEditDto model);
        Task<ProblemDetails<string>> AddMenu(MenuAddDto model);
        Task<IEnumerable<MenuDto>> GetMenuDtoByRoleIdAsync(int Id);
        Task<ProblemDetails<string>> Delete(string Id);
        Task<ProblemDetails<string>> QuickEditMenu(MenuQuickEditDto menuQuickEditDto);
    }
}
