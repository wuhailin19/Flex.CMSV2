﻿using Flex.Application.Contracts.IServices.Basics;
using Flex.Core.Config;
using Flex.Domain.Base;
using Flex.Domain.Dtos.Role;

namespace Flex.Application.Services
{
    public abstract class BaseService : IBaseService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        public readonly IdWorker _idWorker;
        public readonly IClaimsAccessor _claims;
        protected ProblemDetails<T> Problem<T>(HttpStatusCode? statusCode, string detail = null, Exception exception = null)
        {
            if (statusCode == HttpStatusCode.OK)
                return new ProblemDetails<T>(statusCode, default, detail);
            if (statusCode == HttpStatusCode.InternalServerError)
                throw new AopHandledException(detail, exception);
            throw new WarningHandledException(detail, exception);
        }
        protected ProblemDetails<T> TaskProblem<T>(HttpStatusCode? statusCode, long taskId, string detail = null, Exception exception = null)
        {
            throw new TaskHandledException(detail, taskId, exception);
        }
        protected ProblemDetails<T> Problem<T>(HttpStatusCode? statusCode, T Content, string detail = null) => new ProblemDetails<T>(statusCode, Content, detail);
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idWorker = idWorker;
            _claims = claims;
        }
        protected string DecodeData(string data)
        {
            string temp = data;

            temp = temp.Replace("**!1**", "'");
            temp = temp.Replace("**!2**", "(");
            temp = temp.Replace("**!3**", ")");
            temp = temp.Replace("**!4**", "..");
            temp = temp.Replace("**!6**", "\"");
            temp = temp.Replace("**!8**", "<");
            temp = temp.Replace("**!9**", ">");
            temp = temp.Replace("**!10**", "|");
            temp = temp.Replace("**!11**", "\\");
            temp = temp.Replace("**!12**", "+");
            temp = temp.Replace("**!14**", "@");
            temp = temp.Replace("**!15**", "$");
            temp = temp.Replace("**!16**", ":");
            temp = temp.Replace("**!18**", " a");
            temp = temp.Replace("**!19**", " A");
            temp = temp.Replace("**!20**", "/**/");

            return temp;
        }
        protected DataPermissionDto GetSitePermissionDto(string datapermission, int siteId = 0)
        {
            var sitepermissionmodel = JsonHelper.Json<List<sitePermissionDto>>(datapermission ?? string.Empty);
            if (sitepermissionmodel == null)
                return new DataPermissionDto();
            var resultmodel = sitepermissionmodel.Where(m => m.siteId == (siteId == 0 ? CurrentSiteInfo.SiteId : siteId)).FirstOrDefault();
            if (resultmodel == null)
            {
                return new DataPermissionDto();
            }
            return resultmodel.columnPermission;
        }
        public virtual void AddIntEntityBasicInfo<T>(T model) where T : BaseIntEntity
        {
            model.AddUser = _claims.UserId;
            model.AddUserName = _claims.UserName;
            model.AddTime = Clock.Now;
        }
        public virtual void AddLongEntityBasicInfo<T>(T model) where T : BaseLongEntity
        {
            model.Id = _idWorker.NextId();
            model.AddUser = _claims.UserId;
            model.AddUserName = _claims.UserName;
            model.AddTime = Clock.Now;
        }
        public virtual void AddStringEntityBasicInfo<T>(T model) where T : BaseEntity
        {
            model.AddUser = _claims.UserId;
            model.AddUserName = _claims.UserName;
        }
        public virtual void AddFiledEntityBasicInfo<T>(T model) where T : BaseEntity
        {
            model.AddUser = _claims.UserId;
            model.AddTime = Clock.Now;
            model.LastEditDate = Clock.Now;
            model.StatusCode = StatusCode.Enable;
            model.Version = 0;
            model.AddUserName = _claims.UserName;
        }
        public virtual void UpdateLongEntityBasicInfo<T>(T model) where T : BaseLongEntity
        {
            model.Version += 1;
            model.LastEditDate = Clock.Now;
            model.LastEditUser = _claims.UserId;
            model.LastEditUserName = _claims.UserName;
        }
        public virtual void UpdateIntEntityBasicInfo<T>(T model) where T : BaseIntEntity
        {
            model.Version += 1;
            model.LastEditDate = Clock.Now;
            model.LastEditUser = _claims.UserId;
            model.LastEditUserName = _claims.UserName;
        }
        public virtual void UpdateStringEntityBasicInfo<T>(T model) where T : BaseEntity
        {
            model.Version += 1;
            model.LastEditDate = Clock.Now;
            model.LastEditUser = _claims.UserId;
            model.LastEditUserName = _claims.UserName;
        }
    }
}
