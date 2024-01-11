using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IContentModelServices
    {
        Task<ProblemDetails<string>> Add(AddContentModelDto model);
        Task<ProblemDetails<string>> Delete(string Id);
        Task<IEnumerable<ContentSelectItemDto>> GetSelectItem();
        Task<IEnumerable<ContentModelColumnDto>> ListAsync();
        Task<ProblemDetails<string>> Update(UpdateContentModelDto model);
    }
}
