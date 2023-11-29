using Flex.EFSqlServer.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.EFSqlServer.Transaction
{
    public interface IEfCoreIUnitWork
    {
        bool IsStartingUow { get; }
        void BeginTrans(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool distributed = false);
        void Commit();
        /// <summary>
        /// 获取指定仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IEfCoreRespository<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task CommitAsync(CancellationToken cancellationToken = default);
        void Dispose();
        void Rollback();
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
