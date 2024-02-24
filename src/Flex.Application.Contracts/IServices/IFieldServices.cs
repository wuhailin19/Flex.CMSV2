using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IFieldServices
    {
        Task<ProblemDetails<string>> Add(AddFieldDto model);
        Task<ProblemDetails<string>> Delete(string Id);
        Task<UpdateFieldDto> GetFiledInfoById(string Id);
        Task<IEnumerable<FieldColumnDto>> ListAsync(int Id);
        Task<ProblemDetails<string>> QuickEditField(FiledQuickEditDto fieldQuickEditDto);
        Task<ProblemDetails<string>> Update(UpdateFieldDto updateFieldDto);
    }
}
