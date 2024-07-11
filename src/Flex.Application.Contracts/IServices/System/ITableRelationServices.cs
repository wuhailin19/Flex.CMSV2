using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.System.TableRelation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices.System
{
    public interface ITableRelationServices
    {
        Task<ProblemDetails<string>> AddTableRelation(AddTableRelationDto addDto);
        Task<ProblemDetails<string>> DeleteTableRelation(string Id);
        Task<PagedList<TableRelationColumnDto>> ListAsync(int page, int pagesize);
        Task<ProblemDetails<string>> QuickEdit(QuickEditTableRelationDto model);
        Task<ProblemDetails<string>> UpdateTableRelation(UpdateTableRelationDto updateDto);
    }
}
