﻿using Flex.EFSql.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Flex.EFSql.UnitOfWork
{
    /// <summary>
    /// 定义工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// 获取指定仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="hasCustomRepository">如有自定义仓储设为True</param>
        /// <returns></returns>
        IEfCoreRespository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;

        /// <summary>
        /// DbContext提交修改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// DbContext提交修改（异步）
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 执行原生sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 使用原生sql查询来获取指定数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;
        void SetTransaction();
        void SaveChangesTran();
        Task SaveChangesTranAsync();
        void Rollback();
        Task RollbackAsync();
    }
}
