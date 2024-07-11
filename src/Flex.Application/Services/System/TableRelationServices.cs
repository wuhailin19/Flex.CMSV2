using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices.System;
using Flex.Core.Config;
using Flex.Domain.Dtos;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.Normal.ProductManage;
using Flex.Domain.Dtos.System.ContentModel;
using Flex.Domain.Dtos.System.TableRelation;
using Flex.Domain.Entities.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services.System
{
    public class TableRelationServices : BaseService, ITableRelationServices
    {
        //获取本站或者公有的模型
        protected Expression<Func<sysTableRelation, bool>> expression = m => m.SiteId == CurrentSiteInfo.SiteId || !m.SelfUse;
        public TableRelationServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {

        }
        public async Task<PagedList<TableRelationColumnDto>> ListAsync(int page, int pagesize)
        {
            Func<IQueryable<sysTableRelation>, IOrderedQueryable<sysTableRelation>> orderby
                = new Func<IQueryable<sysTableRelation>, IOrderedQueryable<sysTableRelation>>(m => m.OrderByDescending(m => m.AddTime));
            var list = await _unitOfWork
                .GetRepository<sysTableRelation>()
                .GetPagedListAsync(expression, orderby, null, page, pagesize);
            return _mapper.Map<PagedList<TableRelationColumnDto>>(list);
        }

        public async Task<ProblemDetails<string>> QuickEdit(QuickEditTableRelationDto model)
        {
            var responsity = _unitOfWork.GetRepository<sysTableRelation>();
            var contentmodel = await responsity.GetFirstOrDefaultAsync(m => m.Id == model.Id);
            contentmodel.SelfUse = model.SelfUse;
            UpdateIntEntityBasicInfo(contentmodel);
            try
            {
                responsity.Update(contentmodel);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> AddTableRelation(AddTableRelationDto addDto)
        {
            var coreRespository = _unitOfWork.GetRepository<sysTableRelation>();
            var model = _mapper.Map<sysTableRelation>(addDto);
            AddIntEntityBasicInfo(model);
            try
            {
                if (CurrentSiteInfo.SiteId == 0)
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataExist.GetEnumDescription());
                model.SiteId = CurrentSiteInfo.SiteId;
                model.SelfUse = true;
                coreRespository.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataInsertError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> UpdateTableRelation(UpdateTableRelationDto updateDto)
        {
            var coreRespository = _unitOfWork.GetRepository<sysTableRelation>();
            var model = await coreRespository.GetFirstOrDefaultAsync(m => m.Id == updateDto.Id);
            _mapper.Map(updateDto, model);
            UpdateIntEntityBasicInfo(model);
            try
            {
                coreRespository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataInsertError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> DeleteTableRelation(string Id)
        {
            var repository = _unitOfWork.GetRepository<sysTableRelation>();
            if (Id.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = repository.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<sysTableRelation>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                }
                repository.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(), ex);
            }
        }
    }
}
