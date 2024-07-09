using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices.System;
using Flex.Core.Config;
using Flex.Core.Extensions;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.System.Column;
using Flex.Domain.Dtos.System.ContentModel;
using Flex.Domain.Dtos.System.Menu;
using Flex.Domain.Enums.LogLevel;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System.Collections;
using System.Linq.Expressions;

namespace Flex.Application.Services
{

    public class ColumnServices : BaseService, IColumnServices
    {
        IRoleServices _roleServices;
        protected ISystemLogServices _logServices;
        private ISystemIndexSetServices _systemIndexSetServices;
        public ColumnServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims
            , IRoleServices roleServices, ISystemIndexSetServices systemIndexSetServices, ISystemLogServices logServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _roleServices = roleServices;
            _systemIndexSetServices = systemIndexSetServices;
            _logServices = logServices;
        }

        //获取本站或者公有的模型
        protected Expression<Func<SysColumn, bool>> expression = m => m.SiteId == CurrentSiteInfo.SiteId;

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
            var list = (await coreRespository.GetAllAsync(expression)).OrderBy(m => m.OrderId).ToList();
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

        public async Task<IEnumerable<TreeColumnListDto>> GetTreeListBySiteIdAsync(int SiteId, int modelId)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            if (SiteId != 0)
                expression = m => m.SiteId == SiteId;
            var list = (await coreRespository.GetAllAsync(expression)).OrderBy(m => m.OrderId).ToList();
            if (!_claims.IsSystem)
            {
            }
            List<TreeColumnListDto> treeColumns = new List<TreeColumnListDto>();
            list.Where(m => m.ModelId == modelId).Each(item =>
            {
                item.CanCopy = true;
                item.disabled = false;
            });
            list.Where(m => m.ParentId == 0).Each(item =>
            {
                treeColumns.Add(_mapper.Map<TreeColumnListDto>(item));
            });
            var cancopylist = list.Where(m => m.ModelId == modelId).ToList();
            AddChildrenToColumn(list, treeColumns);

            return treeColumns;
        }

        public async Task<IEnumerable<TreeColumnListDto>> GetManageTreeListAsync()
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var list = (await coreRespository.GetAllAsync(expression)).OrderBy(m => m.OrderId).ToList();
            if (!_claims.IsSystem)
            {
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                var jObj = GetSitePermissionDto(currentrole.DataPermission);
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
            var list = (await coreRespository.GetAllAsync(expression)).OrderBy(m => m.OrderId).ToList();
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
                var currentexpression = expression;
                currentexpression = currentexpression.And(m => m.ModelId != 0);
                lists = _mapper.Map<List<TreeColumnListDto>>(await repository.GetAllAsync(currentexpression));
            }
            else
            {
                var roles = await _roleServices.GetCurrentRoldDtoAsync();
                var datamission = roles.DataPermission;
                var jObj = GetSitePermissionDto(datamission);
                lists = _mapper.Map<List<TreeColumnListDto>>(await repository.GetAllAsync(m => jObj.sp.ToList("-").Contains(m.Id.ToString()) && m.ModelId != 0));
            }
            var systemindexset = await _systemIndexSetServices.GetbyCurrentIdAsync();

            switch (mode)
            {
                case "5":
                    if (systemindexset.Shortcut.IsNullOrEmpty())
                        return new List<TreeColumnListDto>();
                    return lists.Where(m => systemindexset.Shortcut.ToList().Contains(m.id.ToString()));
                case "6":
                    if (systemindexset.Shortcut.IsNullOrEmpty())
                        return lists;
                    return lists.Where(m => !systemindexset.Shortcut.ToList().Contains(m.id.ToString()));
            }
            return new List<TreeColumnListDto>();
        }


        public async Task<IEnumerable<ColumnSortListDto>> GetColumnSortListByParentIdAsync(int parentId = 0)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            expression = expression.And(m => m.ParentId == parentId);
            var list = (await coreRespository.GetAllAsync(expression)).OrderBy(m => m.OrderId).ToList();

            List<ColumnSortListDto> treeColumns = [.. _mapper.Map<List<ColumnSortListDto>>(list)];
            if (_claims.IsSystem)
            {
                return treeColumns;
            }
            else
            {
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                var jObj = GetSitePermissionDto(currentrole.DataPermission);
                var editstr = jObj.ed.ToList("-");
                treeColumns = treeColumns.Where(m => editstr.Contains(m.Id.ToString())).ToList();
            }
            return treeColumns;
        }

        public async Task<IEnumerable<ColumnListDto>> ListAsync()
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var list = (await coreRespository.GetAllAsync(expression)).OrderBy(m => m.OrderId).ToList();

            List<ColumnListDto> treeColumns = [.. _mapper.Map<List<ColumnListDto>>(list)];
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
                var jObj = GetSitePermissionDto(currentrole.DataPermission);
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
        
        public async Task<IEnumerable<RoleDataColumnListDto>> DataPermissionListAsync(int siteId)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var siteRespository = _unitOfWork.GetRepository<sysSiteManage>();

            var list = (await coreRespository.GetAllAsync(m => m.SiteId == siteId)).OrderBy(m => m.OrderId).ToList();

            List<RoleDataColumnListDto> treeColumns = [.. _mapper.Map<List<RoleDataColumnListDto>>(list)];
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
                var jObj = GetSitePermissionDto(currentrole.DataPermission);
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

            if (CurrentSiteInfo.SiteId == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotInsertAnySite.GetEnumDescription());
            if (!_unitOfWork.GetRepository<sysSiteManage>().Exists())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotInsertAnySite.GetEnumDescription());
            model.SiteId = CurrentSiteInfo.SiteId;
            try
            {
                coreRespository.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                await _logServices.AddContentLog(SystemLogLevel.Normal, $"新增栏目【{model.Name}】", "新增");
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataInsertError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> AppendColumn(AppendColumnDto appendColumnDto)
        {
            if (CurrentSiteInfo.SiteId == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotInsertAnySite.GetEnumDescription());
            if (!_unitOfWork.GetRepository<sysSiteManage>().Exists())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotInsertAnySite.GetEnumDescription());

            var coreRespository = _unitOfWork.GetRepository<SysColumn>();

            var columnlist = appendColumnDto.ColumnList.ToList("\n");
            if (columnlist.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.ColumnListEmpty.GetEnumDescription());

            var columnhash = new Hashtable { [0] = appendColumnDto.ParentId };

            int orderId = 0;
            var orderItem = await coreRespository.GetFirstOrDefaultAsync(null, m => m.OrderByDescending(q => q.OrderId));
            if (orderItem != null)
                orderId = orderItem.OrderId;
            foreach (var item in columnlist)
            {
                if (item.IsNullOrEmpty()) continue;

                var count = item.GetStartCount('-');
                var currentName = item.Substring(count, item.Length - count);
                if (!columnhash.ContainsKey(count))
                {
                    continue;
                }
                var model = new SysColumn
                {
                    Name = currentName,
                    ParentId = appendColumnDto.ParentId == -1 ? 0 : columnhash[count].ToInt(),
                    SiteId = CurrentSiteInfo.SiteId,
                    SeoTitle = currentName,
                    SeoKeyWord = currentName,
                    SeoDescription = currentName,
                    ModelId = appendColumnDto.ModelId,
                    ReviewMode = appendColumnDto.ReviewMode,
                    ColumnImage = appendColumnDto.ColumnImage,
                    OrderId = orderId,
                };
                AddIntEntityBasicInfo(model);

                var result = await coreRespository.InsertAsync(model);
                await _unitOfWork.SaveChangesAsync();
                List<SysColumn> columns = new List<SysColumn>();
                if (result.Entity.Id > 0)
                {
                    await _logServices.AddContentLog(SystemLogLevel.Normal, $"新增栏目【{model.Name}】", "批量新增");
                    if (appendColumnDto.ColumnUrl.IsNotNullOrEmpty() && appendColumnDto.ColumnUrl.Contains("{columnId}"))
                    {
                        model.ColumnUrl = appendColumnDto.ColumnUrl.Replace("{columnId}", result.Entity.Id.ToString());
                        columns.Add(model);
                    }
                    columnhash[count + 1] = result.Entity.Id;
                    orderId++;
                }
                if (columns.Count > 0)
                {
                    coreRespository.Update(columns);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
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
                await _logServices.AddContentLog(SystemLogLevel.Warning, $"删除栏目数据，Id为{Id}", "删除");
                return Problem<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> QuickSortColumn(ColumnQuickSortDto columnQuickSortDto)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            if (columnQuickSortDto.IdString.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            var columnIdlist = columnQuickSortDto.IdString.ToList();
            if (columnIdlist.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            try
            {
                var list = (await coreRespository.GetAllAsync(m => columnIdlist.Contains(m.Id.ToString()))).ToList();
                for (int i = 0; i < columnIdlist.Count; i++)
                {
                    var item = list.Where(m => m.Id == columnIdlist[i].ToInt()).FirstOrDefault();
                    if (item == null)
                        continue;
                    item.OrderId = i;
                }
                coreRespository.Update(list);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> QuickEditColumn(ColumnQuickEditDto columnQuickEditDto)
        {
            var coreRespository = _unitOfWork.GetRepository<SysColumn>();
            var model = await coreRespository.GetFirstOrDefaultAsync(m => m.Id == columnQuickEditDto.Id);
            if (columnQuickEditDto.IsShow.IsNotNullOrEmpty())
                model.IsShow = columnQuickEditDto.IsShow.ToBool();
            try
            {
                UpdateIntEntityBasicInfo(model);
                coreRespository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
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
                await _logServices.AddContentLog(SystemLogLevel.Warning, $"修改栏目【{model.Name}】", "修改");
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
