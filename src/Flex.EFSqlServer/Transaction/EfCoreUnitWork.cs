using Flex.EFSqlServer.Repositories;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.EFSqlServer.Transaction
{
    public class EfCoreUnitWork<TDbContext> : IEfCoreIUnitWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly UnitOfWorkStatus _unitOfWorkStatus;
        private readonly ICapPublisher _capPublisher;
        private IDbContextTransaction _dbTransaction;
        protected Dictionary<Type, object> _repositories;

        public bool IsStartingUow => _unitOfWorkStatus.IsStartingUow;

        public EfCoreUnitWork(TDbContext context
        , UnitOfWorkStatus unitOfWorkStatus
        , ICapPublisher capPublisher = null)
        {
            _unitOfWorkStatus = unitOfWorkStatus ?? throw new ArgumentNullException(nameof(unitOfWorkStatus));
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 获取指定仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEfCoreRespository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            Type type = typeof(IEfCoreRespository<TEntity>);
            if (!_repositories.TryGetValue(type, out object repo))
            {
                IEfCoreRespository<TEntity> newRepo = new EfCoreRespository<TEntity>(_dbContext);
                _repositories.Add(type, newRepo);
                return newRepo;
            }
            return (IEfCoreRespository<TEntity>)repo;
        }

        private IDbContextTransaction GetDbContextTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool distributed = false)
        {
            if (_unitOfWorkStatus.IsStartingUow)
                throw new ArgumentException("UnitOfWork Error");
            else
                _unitOfWorkStatus.IsStartingUow = true;

            IDbContextTransaction trans;

            if (distributed)
            {
                if (_capPublisher == null)
                    throw new ArgumentException("CapPublisher is null");
                else
                    trans = _dbContext.Database.BeginTransaction(_capPublisher, false);
            }
            else
                trans = _dbContext.Database.BeginTransaction(isolationLevel);

            return trans;
        }

        public void BeginTrans(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool distributed = false)
        {
            _dbTransaction = GetDbContextTransaction(isolationLevel, distributed);
        }

        public void Commit()
        {
            CheckNotNull(_dbTransaction);

            _dbTransaction.Commit();
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            CheckNotNull(_dbTransaction);

            await _dbTransaction.CommitAsync(cancellationToken);
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public void Rollback()
        {
            CheckNotNull(_dbTransaction);

            _dbTransaction.Rollback();
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            CheckNotNull(_dbTransaction);

            await _dbTransaction.RollbackAsync(cancellationToken);
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public void Dispose()
        {
            var isNotNull = CheckNotNull(_dbTransaction, false);
            if (isNotNull)
            {
                _dbTransaction.Dispose();
                if (_unitOfWorkStatus != null)
                    _unitOfWorkStatus.IsStartingUow = false;
            }
        }
        private bool CheckNotNull(IDbContextTransaction dbContextTransaction, bool isThrowException = true)
        {
            if (dbContextTransaction == null && isThrowException)
                throw new ArgumentNullException(nameof(dbContextTransaction), "IDbContextTransaction is null");

            return dbContextTransaction != null;
        }
    }
}
