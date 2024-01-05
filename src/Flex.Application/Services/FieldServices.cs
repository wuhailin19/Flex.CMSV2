using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices;
using Flex.Application.SetupExtensions;
using Flex.Application.SqlServerSQLString;
using Flex.Domain.Basics;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using Flex.EFSqlServer.Repositories;
using System.Reflection;

namespace Flex.Application.Services
{
    public class FieldServices : BaseService, IFieldServices
    {
        IEfCoreRespository<sysField> responsity;
        ISqlTableServices _sqlServerServices;
        public FieldServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, ISqlTableServices sqlServerServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            responsity = _unitOfWork.GetRepository<sysField>();
            _sqlServerServices = sqlServerServices;
        }
        public async Task<IEnumerable<FieldColumnDto>> ListAsync(int Id)
        {
            var list = await responsity.GetAllAsync(m => m.ModelId == Id);
            return _mapper.Map<List<FieldColumnDto>>(list);
        }
        public async Task<ProblemDetails<string>> Add(AddFieldDto model)
        {
            var fieldmodel = _mapper.Map<sysField>(model);
            var validatemodel = new FiledValidateModel()
            {
                ValidateEmpty = model.ValidateEmpty,
                ValidateNumber = model.ValidateNumber
            };
            fieldmodel.Validation = JsonHelper.ToJson(validatemodel);
            var fieldattritudemodel = new FieldAttritudeModel()
            {
                Width = model.Width,
                Height = model.Height
            };
            fieldmodel.FieldAttritude = JsonHelper.ToJson(fieldattritudemodel);
            fieldmodel.ShowInTable = model.ShowInTable;
            AddIntEntityBasicInfo(fieldmodel);
            var contentresponsity = _unitOfWork.GetRepository<SysContentModel>();
            var contentmodel = await contentresponsity.GetFirstOrDefaultAsync(m => m.Id == fieldmodel.ModelId);
            _unitOfWork.SetTransaction();
            try
            {
               _unitOfWork.ExecuteSqlCommand(_sqlServerServices.InsertTableField(contentmodel.TableName, fieldmodel));

                var field = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == fieldmodel.ModelId)).ToList();
                field.Add(fieldmodel);
                string htmlstring = string.Empty;
                BaseFieldType baseFieldType;
                foreach (var item in field)
                {
                    var itemValidateModel = JsonHelper.Json<FiledValidateModel>(item.Validation);
                    var itemAttritudeModel = JsonHelper.Json<FieldAttritudeModel>(item.FieldAttritude);
                    HtmlTemplateDictInitExtension.fielddict.TryGetValue(item.FieldType, out baseFieldType);
                    if (baseFieldType == null)
                        continue;
                    htmlstring += baseFieldType.ToHtmlString(item, itemValidateModel, itemAttritudeModel);
                }
                contentmodel.FormHtmlString = htmlstring;
                contentresponsity.Update(contentmodel);

                responsity.Insert(fieldmodel);
                await _unitOfWork.SaveChangesTranAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
        }
        public async Task<ProblemDetails<string>> Update(UpdateFieldDto updateFieldDto)
        {
            var model = await responsity.GetFirstOrDefaultAsync(m => m.Id == updateFieldDto.Id);
            model.Name = updateFieldDto.Name;
            model.FieldName = updateFieldDto.FieldName;
            model.FieldType = updateFieldDto.FieldType;
            var validatemodel = new FiledValidateModel()
            {
                ValidateEmpty = updateFieldDto.ValidateEmpty,
                ValidateNumber = updateFieldDto.ValidateNumber
            };
            model.Validation = JsonHelper.ToJson(validatemodel);
            var fieldattritudemodel = new FieldAttritudeModel()
            {
                Width = updateFieldDto.Width,
                Height = updateFieldDto.Height
            };
            model.FieldAttritude = JsonHelper.ToJson(fieldattritudemodel);
            model.FieldDescription = updateFieldDto.FieldDescription;
            model.ApiName = updateFieldDto.ApiName;
            model.IsApiField = updateFieldDto.IsApiField;
            model.IsSearch = updateFieldDto.IsSearch;
            model.ShowInTable = updateFieldDto.ShowInTable;
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
            return _mapper.Map<UpdateFieldDto>(await responsity.GetFirstOrDefaultAsync(m => m.Id == Id));
        }

        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            if (Id.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = responsity.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            var contentresponsity = _unitOfWork.GetRepository<SysContentModel>();
            var softdels = new List<sysField>();
            if (delete_list.Count == 0)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            foreach (var item in delete_list)
            {
                item.StatusCode = StatusCode.Deleted;
                UpdateIntEntityBasicInfo(item);
                softdels.Add(item);
            }
            int modelId = 0;
            string tableName = string.Empty;
            if (softdels.Count > 0)
            {
                modelId = softdels[0].ModelId;
                tableName = (await contentresponsity.GetFirstOrDefaultAsync(m => m.Id == modelId)).TableName;
            }
            _unitOfWork.SetTransaction();
            try
            {
                if (tableName.IsNotNullOrEmpty())
                {
                    _unitOfWork.ExecuteSqlCommand(_sqlServerServices.DeleteTableField(tableName, softdels));
                }
                responsity.Update(softdels);
                await _unitOfWork.SaveChangesTranAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            }
        }
    }
}
