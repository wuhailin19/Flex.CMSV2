using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using Flex.EFSqlServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class FieldServices : BaseService, IFieldServices
    {
        IEfCoreRespository<sysField> responsity;
        public FieldServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            responsity = _unitOfWork.GetRepository<sysField>();
        }
        public async Task<IEnumerable<FieldColumnDto>> ListAsync(int Id)
        {
            var list = await responsity.GetAllAsync(m => m.ModelId == Id);
            return _mapper.Map<List<FieldColumnDto>>(list);
        }
        public async Task<ProblemDetails<string>> Add(AddFieldDto model)
        {
            var contentmodel = _mapper.Map<sysField>(model);
            var validatemodel = new FiledValidateModel()
            {
                ValidateEmpty = model.ValidateEmpty,
                ValidateNumber = model.ValidateNumber
            };
            contentmodel.Validation = JsonHelper.ToJson(validatemodel);
            var fieldattritudemodel = new FieldAttritudeModel()
            {
                Width = model.Width,
                Height = model.Height
            };
            contentmodel.FieldAttritude = JsonHelper.ToJson(fieldattritudemodel);
            AddIntEntityBasicInfo(contentmodel);
            try
            {
                responsity.Insert(contentmodel);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
        }
        public async Task<ProblemDetails<string>> Update(UpdateFieldDto updateFieldDto)
        {
            var model = await responsity.GetFirstOrDefaultAsync(m => m.Id == updateFieldDto.Id);
            model.Name = updateFieldDto.Name;
            model.FieldName = updateFieldDto.FieldName;
            model.FieldType = updateFieldDto.FieldType;
            var validatemodel = new FiledValidateModel() {
                ValidateEmpty = updateFieldDto.ValidateEmpty,
                ValidateNumber = updateFieldDto.ValidateNumber
            };
            model.Validation = JsonHelper.ToJson(validatemodel);
            var fieldattritudemodel = new FieldAttritudeModel() { 
                Width= updateFieldDto.Width,
                Height= updateFieldDto.Height
            };
            model.FieldAttritude = JsonHelper.ToJson(fieldattritudemodel);
            model.FieldDescription = updateFieldDto.FieldDescription;
            model.ApiName = updateFieldDto.ApiName;
            model.IsApiField = updateFieldDto.IsApiField;
            model.IsSearch = updateFieldDto.IsSearch;
            UpdateIntEntityBasicInfo(model);
            try
            {
                responsity.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
        }
        public async Task<UpdateFieldDto> GetFiledInfoById(int Id)
        {
            return  _mapper.Map<UpdateFieldDto>(await responsity.GetFirstOrDefaultAsync(m => m.Id == Id));
        }

        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            if (Id.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = responsity.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            try
            {
                var softdels = new List<sysField>();
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                }
                responsity.Update(softdels);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            }
        }
    }
}
