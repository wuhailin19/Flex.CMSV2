﻿using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Application.Contracts.IServices.Basics;
using Flex.Domain.Collections;
using Flex.Domain.Dtos.Admin;

namespace Flex.Application.Contracts.IServices
{
    public interface IAdminServices:  IBaseService
    {
        Task<ProblemDetails<string>> DeleteAccountListByIdArray(string Id);
        Task<PagedList<AdminColumnDto>> GetAdminListAsync(int page, int pagesize);
        Task<UserData> GetAdminValidateInfoAsync(long id);
        Task<IEnumerable<AdminDto>> GetAsync();
        Task<SimpleAdminDto> GetCurrentAdminInfoAsync();
        Task<AdminEditInfoDto> GetEditDtoInfoByIdAsync(long Id);
        Task<PagedList<AdminDto>> GetPageListAsync(int pagesize = 10);
		Task<ProblemDetails<string>> InsertAdmin(AdminAddDto insertAdmin);
        Task<ProblemDetails<string>> QuickEditAdmin(AdminQuickEditDto adminQuickEditDto);
        Task<ProblemDetails<string>> SimpleEditAdminUpdate(SimpleEditAdminDto simpleEditAdmin);
        Task<ProblemDetails<string>> UpdateAdmin(AdminEditDto editAdmin);
        Task<ProblemDetails<string>> UpdateCurrentAccountPassword(AccountAndPasswordDto accountAndPasswordDto);
    }
}