using Flex.Core.Config;
using Flex.Core.Extensions;
using Flex.Core.Framework.Enum;
using Flex.Core.Helper.LogHelper;
using Flex.Dapper;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SqlSugar;

namespace Flex.SqlSugarFactory.Seed
{
    public class MyContext
    {
        private static DbType _dbType;
        private static string _connectionString
        {
            get
            {
                switch (DataBaseConfig.dataBase)
                {
                    default:
                        _dbType = DbType.SqlServer;
                        return "DataConfig:Sqlserver:ConnectionString".Config(string.Empty);
                    case DataBaseType.Mysql:
                        _dbType = DbType.MySql;
                        return "DataConfig:Mysql:ConnectionString".Config(string.Empty);
                    case DataBaseType.DM:
                        _dbType = DbType.Dm;
                        return "DataConfig:DM8:ConnectionString".Config(string.Empty);
                    case DataBaseType.PgSql:
                        _dbType = DbType.PostgreSQL;
                        return "DataConfig:PostgreSQL:ConnectionString".Config(string.Empty);
                }
            }
        }
        private SqlSugarClient _db;

        /// <summary>
        /// 连接字符串 
        /// Blog.Core
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// 数据连接对象 
        /// Blog.Core 
        /// </summary>
        public SqlSugarClient Db
        {
            get { return _db; }
            private set { _db = value; }
        }

        public Page PageAsync(int pageIndex, int pageSize, string sql, object param = null)
        {
            DapperPage.BuildPageQueries((pageIndex - 1) * pageSize, pageSize, sql, out string sqlCount, out string sqlPage);

            var result = new Page
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = _db.Ado.SqlQuerySingle<int>(sqlCount, param)
            };
            result.TotalPages = result.TotalCount / pageSize;

            if ((result.TotalCount % pageSize) != 0)
                result.TotalPages++;

            result.Items = _db.Ado.SqlQuery<dynamic>(sqlPage, param);
            return result;
        }


        /// <summary>
        /// 功能描述:构造函数
        /// 作　　者:Blog.Core
        /// </summary>
        public MyContext()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException("数据库连接字符串为空");
            var _loger = StaticLoggerFactory.MyLoggerFactory.CreateLogger("SqlSugarContext");
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,//mark
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    //IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                }
            }, db =>
            {
                //如果是多库看标题6
                //每次Sql执行前事件
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    //我可以在这里面写逻辑
                    //获取原生SQL推荐 5.1.4.63  性能OK
                    //Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
                    _loger.Log(LogLevel.Information,UtilMethods.GetNativeSql(sql, pars));
                    //foreach (var item in pars)
                    //{
                    //    Console.WriteLine(item.ToJsonString());
                    //}
                    //获取无参数化SQL 影响性能只适合调试
                    //Console.Write(UtilMethods.GetSqlString(DbType.SqlServer, sql, pars));
                    //技巧：AOP中获取IOC对象
                    //var serviceBuilder = services.BuildServiceProvider();
                    //var log= serviceBuilder.GetService<ILogger<WeatherForecastController>>();
                };
            });
        }
    }
}
