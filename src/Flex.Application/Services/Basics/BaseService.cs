using AutoMapper;
using Flex.Application.Authorize;
using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Application.Contracts.IServices.Basics;
using Flex.Core.IDCode;
using Flex.Domain.Base;
using Flex.EFSqlServer.UnitOfWork;
using System.Net;

namespace Flex.Application.Services
{
    public abstract class BaseService : IBaseService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        public readonly IdWorker _idWorker;
        public readonly IClaimsAccessor _claims;
        protected ProblemDetails<T> Problem<T>(HttpStatusCode? statusCode,string detail=null) => new ProblemDetails<T>(statusCode, default, detail);
        protected ProblemDetails<T> Problem<T>(HttpStatusCode? statusCode, T Content, string detail = null) => new ProblemDetails<T>(statusCode, Content, detail);
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idWorker = idWorker;
            _claims = claims;
        }

        public virtual void AddIntEntityBasicInfo<T>(T model) where T : BaseIntEntity  {
            model.AddUser = _claims.UserId;
            model.AddUserName= _claims.UserName;
        }
        public virtual void AddLongEntityBasicInfo<T>(T model) where T : BaseLongEntity
        {
            model.Id = _idWorker.NextId();
            model.AddUser = _claims.UserId;
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
    }
}
