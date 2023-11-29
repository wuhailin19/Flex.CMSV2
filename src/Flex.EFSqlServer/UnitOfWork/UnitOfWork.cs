using Flex.EFSqlServer.Repositories;
using Flex.EFSqlServer.Transaction;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.EFSqlServer.UnitOfWork
{
    /// <summary>
    /// 工作单元的默认实现.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        protected readonly TContext _context;
        protected bool _disposed = false;
        protected Dictionary<Type, object> _repositories;
        private IDbContextTransaction _dbTransaction;

        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        /// <summary>
        /// 获取DbContext
        /// </summary>
        public TContext DbContext => _context;


        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        #region 事务对象用法
        private bool CheckNotNull(IDbContextTransaction dbContextTransaction, bool isThrowException = true)
        {
            if (dbContextTransaction == null && isThrowException)
                throw new ArgumentNullException(nameof(dbContextTransaction), "IDbContextTransaction is null");

            return dbContextTransaction != null;
        }
        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public void SetTransaction()
        {
            _dbTransaction = _context.Database.BeginTransaction();
        }
        public void SaveChangesTran()
        {
            CheckNotNull(_dbTransaction);
            _context.SaveChanges();
            _dbTransaction.Commit();
        }
        public async Task SaveChangesTranAsync()
        {
            CheckNotNull(_dbTransaction);
            await _context.SaveChangesAsync();
            await _dbTransaction.CommitAsync();
        }
        public void Rollback() {
            CheckNotNull(_dbTransaction);
            _dbTransaction.Rollback();
        }
        public async Task RollbackAsync()
        {
            CheckNotNull(_dbTransaction);
           await _dbTransaction.RollbackAsync();
        }
        #endregion
        /// <summary>
        /// 获取指定仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="hasCustomRepository"></param>
        /// <returns></returns>
        public IEfCoreRespository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            Type type = typeof(IEfCoreRespository<TEntity>);
            if (!_repositories.TryGetValue(type, out object repo))
            {
                IEfCoreRespository<TEntity> newRepo = new EfCoreRespository<TEntity>(_context);
                _repositories.Add(type, newRepo);
                return newRepo;
            }
            return (IEfCoreRespository<TEntity>)repo;
        }

        /// <summary>
        /// 执行原生sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlRaw(sql, parameters);

        /// <summary>
        /// 使用原生sql查询来获取指定数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => _context.Set<TEntity>().FromSqlRaw(sql, parameters);

        /// <summary>
        /// DbContext提交修改
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// DbContext提交修改（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            var isNotNull = CheckNotNull(_dbTransaction, false);
            if (isNotNull)
            {
                _dbTransaction.Dispose();
            }
            if (!_disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (_repositories != null)
                    {
                        _repositories.Clear();
                    }

                    // dispose the db context.
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

    }
}
