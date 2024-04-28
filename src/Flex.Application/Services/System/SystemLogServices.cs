using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices.System;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.System.SystemLog;
using Flex.Domain.Enums.LogLevel;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Flex.Application.Services.System
{
    public class SystemLogServices : BaseService, ISystemLogServices
    {
        public SystemLogServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
        public async Task<PagedList<SystemLogColumnDto>> ListAsync(int page, int limit, LogSort LogSort, SystemLogLevel LogLevel = SystemLogLevel.All, string msg = null)
        {
            Expression<Func<sysSystemLog, bool>> extension = null;
            extension = m => (int)m.LogSort == (int)LogSort;
            if (LogLevel == SystemLogLevel.All)
            {
                if (!_claims.IsSystem)
                    extension = extension.And(m => m.AddUser == _claims.UserId);
            }
            else
            {
                if (!_claims.IsSystem)
                    extension = extension.And(m => (int)m.LogLevel == (int)LogLevel && m.AddUser == _claims.UserId);
                else
                    extension = extension.And(m => (int)m.LogLevel == (int)LogLevel);
            }
            if (!msg.IsNullOrEmpty())
                extension = extension.And(m => m.OperationContent.Contains(msg)
                || m.Operator.Contains(msg)
                || m.RoleName.Contains(msg)
                || m.Url.Contains(msg));
            Func<IQueryable<sysSystemLog>, IOrderedQueryable<sysSystemLog>> orderBy = m => m.OrderByDescending(n => n.AddTime);
            var list = await _unitOfWork.GetRepository<sysSystemLog>().GetPagedListAsync(extension, orderBy, null, page, limit);
            return _mapper.Map<PagedList<SystemLogColumnDto>>(list);
        }
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task AddLog(InputSystemLogDto log)
        {
            var insertmodel = _mapper.Map<sysSystemLog>(log);
            AddLongEntityBasicInfo(insertmodel);
            insertmodel.Ip = AcbHttpContext.ClientIp;
            insertmodel.RoleName = _claims.UserRoleDisplayName;
            insertmodel.Operator = _claims.UserName + $"({_claims.UserId})";
            try
            {
                var result = await _unitOfWork.GetRepository<sysSystemLog>().InsertAsync(insertmodel);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                //跳过
            }
        }
        /// <summary>
        /// 数据操作日志
        /// </summary>
        /// <param name="systemLogLevel"></param>
        /// <param name="operationContent"></param>
        /// <param name="request">使用方法</param>
        /// <returns></returns>
        public async Task AddContentLog(SystemLogLevel systemLogLevel, string operationContent, string request)
        {
            var insertmodel = new sysSystemLog();
            AddLongEntityBasicInfo(insertmodel);
            insertmodel.Ip = AcbHttpContext.ClientIp;
            insertmodel.RoleName = _claims.UserRoleDisplayName;
            insertmodel.Operator = _claims.UserName + $"({_claims.UserId})";
            insertmodel.LogLevel = systemLogLevel;
            insertmodel.Url = request;
            insertmodel.LogSort = LogSort.DataOperation;
            insertmodel.OperationContent = operationContent;
            try
            {
                var result = await _unitOfWork.GetRepository<sysSystemLog>().InsertAsync(insertmodel);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                //跳过
            }
        }
        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="loginSystemLogDto"></param>
        /// <returns></returns>
        public async Task AddLoginLog(LoginSystemLogDto loginSystemLogDto)
        {
            var insertmodel = new sysSystemLog();
            insertmodel.Ip = AcbHttpContext.ClientIp;
            insertmodel.RoleName = "";
            insertmodel.Operator = loginSystemLogDto.inoperator.IsNullOrEmpty() ? "用户未认证" : loginSystemLogDto.inoperator;
            insertmodel.LogLevel = loginSystemLogDto.systemLogLevel;
            insertmodel.Url = "登录接口";
            insertmodel.LogSort = LogSort.Login;
            insertmodel.OperationContent = loginSystemLogDto.operationContent;
            insertmodel.AddTime = DateTime.Now;
            insertmodel.Id = _idWorker.NextId();
            if (loginSystemLogDto.IsAuthenticated)
            {
                insertmodel.AddUser = loginSystemLogDto.UserId;
                insertmodel.AddUserName = loginSystemLogDto.UserName;
            }
            try
            {
                var result = await _unitOfWork.GetRepository<sysSystemLog>().InsertAsync(insertmodel);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                //跳过
            }
        }
    }
}
