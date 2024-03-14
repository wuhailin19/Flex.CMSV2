using Flex.Core.Extensions;
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
                var usedb = "DataConfig:UseDb".Config(string.Empty) ?? "Sqlserver";
                switch (usedb)
                {
                    default:
                        _dbType = DbType.SqlServer;
                        return "DataConfig:Sqlserver:ConnectionString".Config(string.Empty);
                    case "Mysql":
                        _dbType = DbType.MySql;
                        return "DataConfig:Mysql:ConnectionString".Config(string.Empty);
                    case "DM8":
                        _dbType = DbType.Dm;
                        return "DataConfig:DM8:ConnectionString".Config(string.Empty);
                    case "PgSql":
                        _dbType = DbType.PostgreSQL;
                        return "DataConfig:DM8:ConnectionString".Config(string.Empty);
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

        /// <summary>
        /// 功能描述:构造函数
        /// 作　　者:Blog.Core
        /// </summary>
        public MyContext()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException("数据库连接字符串为空");
            _db= new SqlSugarClient(new ConnectionConfig()
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
            }) ;
        }
    }
}
