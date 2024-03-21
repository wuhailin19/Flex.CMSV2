using Flex.Application.SqlServerSQLString;
using Flex.Core.Config;
using Flex.Core.Framework.Enum;
using Flex.Dapper;
using Flex.EFSql;
using Flex.EFSql.Register;
using Flex.SqlSugarFactory.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Flex.Application.SetupExtensions.OrmInitExtension
{
    //新增数据库判断，新增PostgreSQL,Mysql,达梦版本语句
    public static class OrmInitExtension
    {
        public static void RegisterDbConnectionString(this IServiceCollection services) {

            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.SqlServer:
                    services.AddUnitOfWorkService<EfCoreDBContext>(item => item.UseSqlServer($"DataConfig:Sqlserver:ConnectionString".Config(string.Empty)));
                    services.AddScoped<ISqlTableServices, SqlServerSqlTableServices>();
                    break;
                case DataBaseType.Mysql:
                    services.AddUnitOfWorkService<EfCoreDBContext>(item => item.UseMySql($"DataConfig:Mysql:ConnectionString".Config(string.Empty), new MySqlServerVersion(new Version("8.0.27"))));
                    services.AddScoped<ISqlTableServices, MySqlSqlTableServices>();
                    break;
                case DataBaseType.DM:
                    services.AddUnitOfWorkService<EfCoreDBContext>(item => item.UseDm($"DataConfig:DM8:ConnectionString".Config(string.Empty)));
                    services.AddScoped<ISqlTableServices, DamengSqlTableServices>();
                    break;
                case DataBaseType.PgSql:
                    services.AddUnitOfWorkService<EfCoreDBContext>(item => item.UseNpgsql($"DataConfig:PostgreSQL:ConnectionString".Config(string.Empty)));
                    services.AddScoped<ISqlTableServices, PostgreSqlTableServices>();
                    break;
            }
            //注册sqlsugar
            services.AddSingleton<MyContext>();
            //注册dapper
            services.AddSingleton<MyDBContext>();
        }
    }
}
