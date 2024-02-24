using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.RoleUrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IRoleUrlServices
    {
        Task<ProblemDetails<string>> CreateUrlList();
        Task<IEnumerable<RoleUrlListDto>> GetApiUrlListByCateId(string cateid, string k = null);
        Task<ApiPermissionDto> GetRoleUrlListById(int Id);
    }
}
