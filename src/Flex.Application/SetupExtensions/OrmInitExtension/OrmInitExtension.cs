using Flex.Application.SqlServerSQLString;
using Flex.Dapper;
using Flex.EFSql;
using Flex.EFSql.Register;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Flex.Application.SetupExtensions.OrmInitExtension
{
    //新增数据库判断，新增PostgreSQL,Mysql,达梦版本语句
    public static class OrmInitExtension
    {
        public static void RegisterDbConnectionString(this IServiceCollection services) {

            var usedb = "DataConfig:UseDb".Config(string.Empty) ?? "Sqlserver";
            switch (usedb)
            {
                case "Sqlserver":
                    services.AddUnitOfWorkService<SqlServerContext>(item => item.UseSqlServer($"DataConfig:Sqlserver:ConnectionString".Config(string.Empty)));
                    services.AddScoped<ISqlTableServices, SqlServerSqlTableServices>();
                    break;
                case "Mysql":
                    services.AddUnitOfWorkService<SqlServerContext>(item => item.UseMySql($"DataConfig:Mysql:ConnectionString".Config(string.Empty), new MySqlServerVersion(new Version("8.0.27"))));
                    services.AddScoped<ISqlTableServices, MySqlSqlTableServices>();
                    break;
                case "DM8":
                    services.AddUnitOfWorkService<SqlServerContext>(item => item.UseSqlServer($"DataConfig:DM8:ConnectionString".Config(string.Empty)));
                    services.AddScoped<ISqlTableServices, DamengSqlTableServices>();
                    break;
                case "PgSql":
                    services.AddUnitOfWorkService<SqlServerContext>(item => item.UseSqlServer($"DataConfig:PostgreSQL:ConnectionString".Config(string.Empty)));
                    services.AddScoped<ISqlTableServices, PostgreSqlTableServices>();
                    break;
            }

            services.AddSingleton<MyDBContext>();
        }
    }
}
