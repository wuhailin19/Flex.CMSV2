using Flex.Core.Extensions;
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
            var usedb = "DataConfig:UseDb".Config(string.Empty) ?? "Sqlserver";
            var sqlconnection = "DataConfig:Sqlserver:ConnectionString".Config(string.Empty);
            switch (usedb)
            {
                case "Sqlserver":
                    return new SqlConnection(sqlconnection);
                case "Mysql":
                    sqlconnection = "DataConfig:Mysql:ConnectionString".Config(string.Empty);
                    return new MySql.Data.MySqlClient.MySqlConnection(sqlconnection);
                case "DM8":
                    sqlconnection = "DataConfig:DM8:ConnectionString".Config(string.Empty);
                    return new Dm.DmConnection(sqlconnection);
                case "PgSql":
                    sqlconnection = "DataConfig:PostgreSQL:ConnectionString".Config(string.Empty);
                    return new SqlConnection(sqlconnection);
                default:
                    return new SqlConnection(sqlconnection);
            }
        }


    }
}
