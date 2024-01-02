using Flex.Application.Contracts.Exceptions;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class ColumnServices : BaseService, IColumnServices
    {
        public ColumnServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }

        private void AddChildrenToColumn(List<SysColumn> fulllist, List<TreeColumnListDto> treeColumns)
        {
            List<SysColumn> sysColumns = new List<SysColumn>();
            foreach (var item in treeColumns)
            {
                sysColumns = fulllist.Where(m => m.ParentId == item.id).ToList();
                if (sysColumns.Count > 0)
                {
                    item.children = _mapper.Map<List<TreeColumnListDto>>(sysColumns);
                    AddChildrenToColumn(fulllist, item.children);
                }
            }
        }

        public async Task<IEnumerable<TreeColumnListDto>> GetTreeColumnListDtos()
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var list = (await coreRespository.GetAllAsync()).OrderBy(m => m.OrderId).ToList();
            if (!_claims.IsSystem)
            {
            }
            List<TreeColumnListDto> treeColumns = new List<TreeColumnListDto>();
            list.Where(m => m.ParentId == 0).Each(item =>
            {
                treeColumns.Add(_mapper.Map<TreeColumnListDto>(item));
            });
            AddChildrenToColumn(list, treeColumns);
            var finaltrees = new List<TreeColumnListDto>
            {
                new TreeColumnListDto() { id = 0, ParentId = -2, title = "根节点", children = treeColumns }
            };
            return finaltrees;
        }
        
        public async Task<IEnumerable<TreeColumnListDto>> GetManageTreeListAsync()
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var list = (await coreRespository.GetAllAsync()).OrderBy(m => m.OrderId).ToList();
            if (!_claims.IsSystem)
            {
            }
            List<TreeColumnListDto> treeColumns = new List<TreeColumnListDto>();
            list.Where(m => m.ParentId == 0).Each(item =>
            {
                treeColumns.Add(_mapper.Map<TreeColumnListDto>(item));
            });
            AddChildrenToColumn(list, treeColumns);
            return treeColumns;
        }

        public async Task<IEnumerable<TreeColumnListDto>> GetTreeSelectListDtos()
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var list = (await coreRespository.GetAllAsync()).OrderBy(m => m.OrderId).ToList();
            if (!_claims.IsSystem)
            {
            }
            List<TreeColumnListDto> treeColumns = new List<TreeColumnListDto> {
                 new TreeColumnListDto() { id = 0, ParentId = -2, title = "根节点" }
            };
            list.Each(item =>
            {
                treeColumns.Add(_mapper.Map<TreeColumnListDto>(item));
            });
            return treeColumns;
        }

        public async Task<IEnumerable<ColumnListDto>> ListAsync()
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var list = (await coreRespository.GetAllAsync()).OrderBy(m => m.OrderId).ToList();

            List<ColumnListDto> treeColumns = new List<ColumnListDto>();
            treeColumns.AddRange(_mapper.Map<List<ColumnListDto>>(list));
            if (_claims.IsSystem)
            {
                treeColumns.Each(m =>
                {
                    m.IsDelete = true;
                    m.IsEdit = true;
                    m.IsSelect = true;
                    m.IsAdd = true;
                });
            }
            return treeColumns;
        }
        public async Task<ProblemDetails<string>> AddColumn(AddColumnDto addColumnDto)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var model = _mapper.Map<SysColumn>(addColumnDto);
            AddIntEntityBasicInfo(model);
            try
            {
                coreRespository.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
        }

        public async Task<ProblemDetails<string>> UpdateColumn(UpdateColumnDto updateColumnDto)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var model = await coreRespository.GetFirstOrDefaultAsync(m => m.Id == updateColumnDto.Id);
            model.Id = updateColumnDto.Id;
            model.Name = updateColumnDto.Name;
            model.ColumnImage = updateColumnDto.ColumnImage;
            model.ColumnUrl = updateColumnDto.ColumnUrl;
            model.SeoDescription = updateColumnDto.SeoDescription;
            model.SeoKeyWord = updateColumnDto.SeoKeyWord;
            model.SeoTitle = updateColumnDto.SeoTitle;
            model.OrderId = updateColumnDto.OrderId;
            model.ModelId = updateColumnDto.ModelId;
            model.ExtensionModelId = updateColumnDto.ExtensionModelId;
            model.IsShow = updateColumnDto.IsShow;
            model.ParentId= updateColumnDto.ParentId;
            UpdateIntEntityBasicInfo(model);
            try
            {
                coreRespository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
        }

        public async Task<UpdateColumnDto> GetColumnById(int Id)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var model = await coreRespository.GetFirstOrDefaultAsync(m => m.Id == Id);
            return _mapper.Map<UpdateColumnDto>(model);
        }
    }
}
