using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices;
using Flex.Application.SetupExtensions;
using Flex.Application.SqlServerSQLString;
using Flex.Domain.Basics;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using Flex.EFSql.Repositories;
using System.Reflection;

namespace Flex.Application.Services
{
    public class FieldServices : BaseService, IFieldServices
    {
        IEfCoreRespository<sysField> responsity;
        IEfCoreRespository<SysContentModel> contentresponsity;
        ISqlTableServices _sqlServerServices;
        public FieldServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, ISqlTableServices sqlServerServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            responsity = _unitOfWork.GetRepository<sysField>();
            _sqlServerServices = sqlServerServices;
            contentresponsity = _unitOfWork.GetRepository<SysContentModel>();
        }
        public async Task<IEnumerable<FieldColumnDto>> ListAsync(int Id)
        {
            var list = (await responsity.GetAllAsync(m => m.ModelId == Id)).ToList();
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
            AddStringEntityBasicInfo(fieldmodel);

            var contentmodel = await contentresponsity.GetFirstOrDefaultAsync(m => m.Id == fieldmodel.ModelId);
            _unitOfWork.SetTransaction();
            try
            {
                _unitOfWork.ExecuteSqlCommand(_sqlServerServices.InsertTableField(contentmodel.TableName, fieldmodel));
                responsity.Insert(fieldmodel);
                await _unitOfWork.SaveChangesTranAsync();

                await CreateModelHtmlString(contentmodel);
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        private async Task CreateModelHtmlString(SysContentModel contentModel)
        {
            var field = new List<sysField>();
            field = (await responsity.GetAllAsync(m => m.ModelId == contentModel.Id)).ToList();
        
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
            contentModel.FormHtmlString = htmlstring;
            try
            {
                _unitOfWork.GetRepository<SysContentModel>().Update(contentModel);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ProblemDetails<string>> QuickEditField(FiledQuickEditDto fieldQuickEditDto)
        {
            var fieldRepository = _unitOfWork.GetRepository<sysField>();
            var model = await fieldRepository.GetFirstOrDefaultAsync(m => m.Id == fieldQuickEditDto.Id);
            if (model is null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (fieldQuickEditDto.IsApiField.IsNotNullOrEmpty())
                model.IsApiField = fieldQuickEditDto.IsApiField.CastTo<bool>();
            if (fieldQuickEditDto.IsSearch.IsNotNullOrEmpty())
                model.IsSearch = fieldQuickEditDto.IsSearch.CastTo<bool>();
            if (fieldQuickEditDto.ShowInTable.IsNotNullOrEmpty())
                model.ShowInTable = fieldQuickEditDto.ShowInTable.CastTo<bool>();
            try
            {
                UpdateStringEntityBasicInfo(model);
                fieldRepository.Update(model);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch(Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(),ex);
            }
        }
        public async Task<ProblemDetails<string>> Update(UpdateFieldDto updateFieldDto)
        {
            var model = await responsity.GetFirstOrDefaultAsync(m => m.Id == updateFieldDto.Id);
            model.Name = updateFieldDto.Name;
            #region 废弃
            //model.FieldName = updateFieldDto.FieldName;
            //model.FieldType = updateFieldDto.FieldType;
            //var validatemodel = new FiledValidateModel()
            //{
            //    ValidateEmpty = updateFieldDto.ValidateEmpty,
            //    ValidateNumber = updateFieldDto.ValidateNumber
            //};
            //model.Validation = JsonHelper.ToJson(validatemodel);
            //var fieldattritudemodel = new FieldAttritudeModel()
            //{
            //    Width = updateFieldDto.Width,
            //    Height = updateFieldDto.Height
            //};
            //model.FieldAttritude = JsonHelper.ToJson(fieldattritudemodel);
            //model.FieldDescription = updateFieldDto.FieldDescription;
            #endregion
            model.ApiName = updateFieldDto.ApiName;
            model.IsApiField = updateFieldDto.IsApiField;
            model.IsSearch = updateFieldDto.IsSearch;
            model.ShowInTable = updateFieldDto.ShowInTable;
            UpdateStringEntityBasicInfo(model);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == model.ModelId);
            try
            {
                responsity.Update(model);
                await _unitOfWork.SaveChangesAsync();

                //await CreateModelHtmlString(contentmodel);
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
        public async Task<UpdateFieldDto> GetFiledInfoById(string Id)
        {
            return _mapper.Map<UpdateFieldDto>(await responsity.GetFirstOrDefaultAsync(m => m.Id == Id));
        }

        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            if (Id.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = responsity.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            var softdels = new List<sysField>();
            if (delete_list.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            foreach (var item in delete_list)
            {
                item.StatusCode = StatusCode.Deleted;
                UpdateStringEntityBasicInfo(item);
                softdels.Add(item);
            }
            int modelId = 0;
            string tableName = string.Empty;

            modelId = softdels[0].ModelId;
            var contentmodel = await contentresponsity.GetFirstOrDefaultAsync(m => m.Id == modelId);
            tableName = contentmodel.TableName;

            _unitOfWork.SetTransaction();
            try
            {
                if (tableName.IsNotNullOrEmpty())
                {
                    _unitOfWork.ExecuteSqlCommand(_sqlServerServices.DeleteTableField(tableName, softdels));
                }
                responsity.Update(softdels);

                await _unitOfWork.SaveChangesTranAsync();

                await CreateModelHtmlString(contentmodel);

                return Problem<string>(HttpStatusCode.OK, $"共删除{Ids.Count}条数据");
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(), ex);
            }
        }
    }
}
