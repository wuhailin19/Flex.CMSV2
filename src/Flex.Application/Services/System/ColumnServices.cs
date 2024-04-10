using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.Role;
using Newtonsoft.Json;
using ShardingCore.Extensions;

namespace Flex.Application.Services
{

    public class ColumnServices : BaseService, IColumnServices
    {
        IRoleServices _roleServices;
        private ISystemIndexSetServices _systemIndexSetServices;
        public ColumnServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, IRoleServices roleServices, ISystemIndexSetServices systemIndexSetServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _roleServices = roleServices;
            _systemIndexSetServices = systemIndexSetServices;
        }

        /// <summary>
        /// 用于递归
        /// </summary>
        /// <param name="fulllist"></param>
        /// <param name="treeColumns"></param>
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
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                var jObj = JsonConvert.DeserializeObject<DataPermissionDto>(currentrole.DataPermission);
                var selectstr = jObj.sp.ToList("-");
                list = list.Where(m => selectstr.Contains(m.Id.ToString())).ToList();
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
        public async Task<IEnumerable<TreeColumnListDto>> getColumnShortcut(string mode = "5")
        {
            var lists = new List<TreeColumnListDto>();
            var repository = _unitOfWork.GetRepository<SysColumn>();
            if (_claims.IsSystem)
            {
                lists = _mapper.Map<List<TreeColumnListDto>>(await repository.GetAllAsync(m => m.ModelId != 0));
            }
            else
            {
                var roles = await _roleServices.GetCurrentRoldDtoAsync();
                var datamission = roles.DataPermission;
                var jObj = JsonConvert.DeserializeObject<DataPermissionDto>(datamission);
                lists = _mapper.Map<List<TreeColumnListDto>>(await repository.GetAllAsync(m => jObj.sp.ToList("-").Contains(m.Id.ToString()) && m.ModelId != 0));
            }
            var systemindexset = await _systemIndexSetServices.GetbyCurrentIdAsync();

            switch (mode)
            {
                case "5":
                    if (systemindexset.Shortcut.IsEmpty())
                        return new List<TreeColumnListDto>();
                    return lists.Where(m => systemindexset.Shortcut.ToList().Contains(m.id.ToString()));
                case "6":
                    if (systemindexset.Shortcut.IsEmpty())
                        return lists;
                    return lists.Where(m => !systemindexset.Shortcut.ToList().Contains(m.id.ToString()));
            }
            return new List<TreeColumnListDto>();
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
            else
            {
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                var jObj = JsonConvert.DeserializeObject<DataPermissionDto>(currentrole.DataPermission);
                var selectstr = jObj.sp.ToList("-");
                treeColumns = treeColumns.Where(m => selectstr.Contains(m.Id.ToString())).ToList();
                foreach (var tc in treeColumns)
                {
                    tc.IsDelete = jObj.dp.ToList("-").Contains(tc.Id.ToString());
                    tc.IsEdit = jObj.ed.ToList("-").Contains(tc.Id.ToString());
                    tc.IsSelect = jObj.sp.ToList("-").Contains(tc.Id.ToString());
                    tc.IsAdd = jObj.ad.ToList("-").Contains(tc.Id.ToString());
                }
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
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataInsertError.GetEnumDescription(),ex);
            }
        }
        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            var adminRepository = _unitOfWork.GetRepository<SysColumn>();
            if (Id.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = adminRepository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<SysColumn>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                }
                adminRepository.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch(Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(),ex);
            }
        }
        public async Task<ProblemDetails<string>> UpdateColumn(UpdateColumnDto updateColumnDto)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var model = await coreRespository.GetFirstOrDefaultAsync(m => m.Id == updateColumnDto.Id);
            _mapper.Map(updateColumnDto, model);
            UpdateIntEntityBasicInfo(model);
            try
            {
                coreRespository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
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
