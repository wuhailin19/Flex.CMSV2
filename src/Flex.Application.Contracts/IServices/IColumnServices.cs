﻿using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IColumnServices
    {
        Task<ProblemDetails<string>> AddColumn(AddColumnDto addColumnDto);
        Task<UpdateColumnDto> GetColumnById(int Id);
        Task<IEnumerable<TreeColumnListDto>> GetManageTreeListAsync();
        Task<IEnumerable<TreeColumnListDto>> GetTreeColumnListDtos();
        Task<IEnumerable<TreeColumnListDto>> GetTreeSelectListDtos();
        Task<IEnumerable<ColumnListDto>> ListAsync();
        Task<ProblemDetails<string>> UpdateColumn(UpdateColumnDto updateColumnDto);
    }
}