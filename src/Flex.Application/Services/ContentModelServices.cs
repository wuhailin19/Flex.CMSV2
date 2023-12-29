using Flex.Application.Contracts.Exceptions;
using Flex.Core.Extensions;
using Flex.Domain.Base;
using Flex.Domain.Dtos.ContentModel;
using Flex.EFSqlServer.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class ContentModelServices : BaseService, IContentModelServices
    {
        public ContentModelServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
        public async Task<IEnumerable<ContentModelColumnDto>> ListAsync()
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var list = await responsity.GetAllAsync();
            return _mapper.Map<List<ContentModelColumnDto>>(list);
        }

        public async Task<ProblemDetails<string>> Add(AddContentModelDto model)
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var contentmodel = _mapper.Map<SysContentModel>(model);
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
        public async Task<ProblemDetails<string>> Update(UpdateContentModelDto model)
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var contentmodel = await responsity.GetFirstOrDefaultAsync(m => m.Id == model.Id);
            contentmodel.Name = model.Name;
            contentmodel.Description = model.Description;
            contentmodel.TableName =
                "tbl_normal_" + model.TableName
                .Replace("tbl_normal_", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            UpdateIntEntityBasicInfo(contentmodel);
            try
            {

                responsity.Update(contentmodel);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
        }

        public string GetFormHtml(params string[] names)
        {
            string htmltemplete = "<div class=\"layui-form-item layui-form-text\">\r\n            <label class=\"layui-form-label\">{0}</label>\r\n            <div class=\"layui-input-block\">\r\n                <input type=\"text\" name=\"{1}\" placeholder=\"请输入{0}\" autocomplete=\"off\" class=\"layui-input\" lay-verify='{2}'>\r\n            </div>\r\n        </div>";
            return string.Format(htmltemplete, names);
        }

        public async Task<ProblemDetails<string>> Delete(string Id)
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            if (Id.IsNullOrEmpty())
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NotChooseData.GetEnumDescription());
            var Ids = Id.ToList("-");
            var delete_list = responsity.GetAll(m => Ids.Contains(m.Id.ToString())).ToList();
            try
            {
                var softdels = new List<SysContentModel>();
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
