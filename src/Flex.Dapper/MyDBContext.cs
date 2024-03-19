using Flex.Core.Config;
using Flex.Core.Extensions;
using Flex.Core.Framework.Enum;
using Flex.Dapper.Context;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace Flex.Dapper
{
    public class MyDBContext : DapperDBContext
    {
        public MyDBContext(IOptions<DapperDBContextOptions> optionsAccessor) : base(optionsAccessor)
        {
            
        }

        protected override IDbConnection CreateConnection()
        {
            var sqlconnection = "DataConfig:Sqlserver:ConnectionString".Config(string.Empty);
            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.SqlServer:
                    return new SqlConnection(sqlconnection);
                case DataBaseType.Mysql:
                    sqlconnection = "DataConfig:Mysql:ConnectionString".Config(string.Empty);
                    return new MySql.Data.MySqlClient.MySqlConnection(sqlconnection);
                case DataBaseType.DM:
                    sqlconnection = "DataConfig:DM8:ConnectionString".Config(string.Empty);
                    return new Dm.DmConnection(sqlconnection);
                case DataBaseType.PgSql:
                    sqlconnection = "DataConfig:PostgreSQL:ConnectionString".Config(string.Empty);
                    return new Npgsql.NpgsqlConnection(sqlconnection);
                default:
                    return new SqlConnection(sqlconnection);
            }
        }
    }
}
