using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.System.SiteManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface ISiteManageServices
    {
        Task<ProblemDetails<string>> AddSite(AddSiteManageDto addmodel);
        Task<ProblemDetails<string>> Delete(string Id);
        Task<IEnumerable<SiteManageColumnDto>> ListAsync();
        Task<ProblemDetails<string>> UpdateSite(UpdateSiteManageDto updatemodel);
    }
}
