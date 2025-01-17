﻿using Flex.Application.ContentModel;
using Flex.Application.Contracts.Exceptions;
using Flex.Core.Config;
using Flex.Domain.Dtos.ContentModel;
using Flex.Domain.Dtos.Field;
using Flex.Domain.Dtos.System.ContentModel;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Flex.Application.Services
{
    public class ContentModelServices : BaseService, IContentModelServices
    {
        ISqlTableServices _sqlServerServices;
        public ContentModelServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, ISqlTableServices sqlServerServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _sqlServerServices = sqlServerServices;
        }
        //获取本站或者公有的模型
        protected Expression<Func<SysContentModel, bool>> expression = m => m.SiteId == CurrentSiteInfo.SiteId || !m.SelfUse;
        public async Task<IEnumerable<ContentModelColumnDto>> ListAsync()
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var list = await responsity.GetAllAsync(expression);
            return _mapper.Map<List<ContentModelColumnDto>>(list);
        }

        public async Task<IEnumerable<ContentSelectItemDto>> GetSelectItem()
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var list = await responsity.GetAllAsync(expression);
            return _mapper.Map<List<ContentSelectItemDto>>(list);
        }
        public async Task<ProblemDetails<string>> GetFormHtml(int ModelId)
        {
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == ModelId);
            if (contentmodel == null)
                return Problem<string>(HttpStatusCode.BadRequest, "没有选择有效模型");
            return Problem<string>(HttpStatusCode.OK, contentmodel.FormHtmlString);
        }
        public async Task<ProblemDetails<string>> Add(AddContentModelDto model)
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var contentmodel = _mapper.Map<SysContentModel>(model);
            AddIntEntityBasicInfo(contentmodel);
            _unitOfWork.SetTransaction();
            try
            {
                if (CurrentSiteInfo.SiteId == 0)
                    return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataExist.GetEnumDescription());
                contentmodel.SiteId = CurrentSiteInfo.SiteId;
                responsity.Insert(contentmodel);
                _unitOfWork.ExecuteSqlCommand(_sqlServerServices.CreateContentTableSql(contentmodel.TableName));
                await _unitOfWork.SaveChangesTranAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataInsertError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<string>> UpdateFormString(UpdateFormHtmlStringDto model)
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var filedresponsity = _unitOfWork.GetRepository<sysField>();

            var contentmodel = await responsity.GetFirstOrDefaultAsync(m => m.Id == model.Id);
            contentmodel.FormHtmlString = model.FormHtmlString?.Replace("\n", "").Replace(" ", "");
            UpdateIntEntityBasicInfo(contentmodel);
            _unitOfWork.SetTransaction();
            var fileds = JsonHelper.Json<List<FiledHtmlStringDto>>(model.FormHtmlString);
            var filedlist = new List<sysField>();
            var insertfiledlist = new List<FiledHtmlStringDto>();
            var updatefiledlist = new List<FiledHtmlStringDto>();
            var deletefiledlist = new List<sysField>();
            try
            {
                var fullfiledlist = (await filedresponsity.GetAllAsync(m => m.ModelId == model.Id)).OrderBy(m=>m.OrderId).ToList();
                #region 对结果集进行分组
                foreach (var item in fileds)
                {
                    if (!fullfiledlist.Any(m => m.Id == item.uuid))
                        insertfiledlist.Add(item);
                    else
                        updatefiledlist.Add(item);
                }
                foreach (var item in fullfiledlist)
                {
                    if (!fileds.Any(m => m.uuid == item.Id))
                        deletefiledlist.Add(item);
                }
                #endregion

                #region 新增部分
                if (insertfiledlist.Count > 0)
                {
                    string filedinsertstring = string.Empty;
                    foreach (var item in insertfiledlist)
                    {
                        var filedmodel = new sysField
                        {
                            Name = item.label,
                            Id = item.uuid,
                            FieldName = item.id,
                            FieldType = item.tag,
                            ModelId = model.Id,
                            OrderId = item.index
                        };
                        AddStringEntityBasicInfo(filedmodel);
                        filedlist.Add(filedmodel);
                    }
                    await filedresponsity.InsertAsync(filedlist);
                    foreach (var item in insertfiledlist)
                    {
                        filedinsertstring = _sqlServerServices.InsertTableField(contentmodel.TableName, item.id, item.tag);
                        _unitOfWork.ExecuteSqlCommand(filedinsertstring);
                    }
                }
                #endregion

                #region 修改部分
                if (updatefiledlist.Count > 0)
                {
                    string updatestring = string.Empty;
                    filedlist.Clear();
                    foreach (var item in updatefiledlist)
                    {
                        var updatemodel = fullfiledlist.Where(m => m.Id == item.uuid)
                            .FirstOrDefault();
                        if (updatemodel != null)
                        {
                            updatemodel.OrderId = item.index;
                            updatemodel.FieldName = item.id;
                            updatemodel.Name = item.label;
                            UpdateStringEntityBasicInfo(updatemodel);
                            filedlist.Add(updatemodel);
                        }
                    }
                    filedresponsity.Update(filedlist);
                    foreach (var item in updatefiledlist)
                    {
                        var updatemodel = fullfiledlist.Where(m => m.Id == item.uuid)
                        .FirstOrDefault();
                        if (updatemodel != null)
                        {
                            if (updatemodel.FieldName != item.id)
                            {
                                updatestring = _sqlServerServices.AlertTableField(contentmodel.TableName, updatemodel.FieldName, item.id);
                                _unitOfWork.ExecuteSqlCommand(updatestring);
                            }
                            if (updatemodel.FieldType != item.tag)
                            {
                                updatestring = _sqlServerServices.AlertTableFieldType(contentmodel.TableName, item.id, item.tag);
                                _unitOfWork.ExecuteSqlCommand(updatestring);
                            }
                        }
                    }
                }
                #endregion

                #region 删除部分
                if (deletefiledlist.Count > 0)
                {
                    string deletestring = string.Empty;
                    foreach (var item in deletefiledlist)
                    {
                        item.StatusCode = StatusCode.Deleted;
                    }
                    filedresponsity.Update(deletefiledlist);
                    foreach (var item in deletefiledlist)
                    {
                        deletestring = _sqlServerServices.ReNameTableField(contentmodel.TableName, item.FieldName, (item.FieldName + item.Id).Replace("-", "_"));
                        _unitOfWork.ExecuteSqlCommand(deletestring);
                    }
                }
                #endregion

                responsity.Update(contentmodel);
                ContentModelHelper.clearData();
                _unitOfWork.SaveChangesTran();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<string>> Update(UpdateContentModelDto model)
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var contentmodel = await responsity.GetFirstOrDefaultAsync(m => m.Id == model.Id);
            string oldtablename = contentmodel.TableName;
            contentmodel.Name = model.Name;
            contentmodel.Description = model.Description;
            contentmodel.SelfUse = model.SelfUse;
            contentmodel.TableName =
                "tbl_normal_" + model.TableName
                .Replace("tbl_normal_", "", RegexOptions.IgnoreCase);
            UpdateIntEntityBasicInfo(contentmodel);
            try
            {
                string renametablesql = _sqlServerServices.ReNameTableName(oldtablename, contentmodel.TableName);
                if (renametablesql.IsNotNullOrEmpty())
                    _unitOfWork.ExecuteSqlCommand(renametablesql);
                responsity.Update(contentmodel);
                await _unitOfWork.SaveChangesAsync();
                return Problem<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataUpdateError.GetEnumDescription(), ex);
            }
        }

        public async Task<ProblemDetails<string>> QuickEdit(QuickEditContentModelDto model)
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
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

        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            if (Id.IsNullOrEmpty())
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = responsity.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            if (delete_list.Count == 0)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            try
            {
                var softdels = new List<SysContentModel>();
                string renametablesql = string.Empty;
                foreach (var item in delete_list)
                {
                    item.StatusCode = StatusCode.Deleted;
                    UpdateIntEntityBasicInfo(item);
                    softdels.Add(item);
                    renametablesql = _sqlServerServices.ReNameTableName(item.TableName, item.TableName + _idWorker.NextId());
                    _unitOfWork.ExecuteSqlCommand(renametablesql);
                }
                responsity.Update(softdels);
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
