using Flex.Core;
using Flex.Core.Extensions;
using Flex.Domain.Base;
using Flex.Domain.Config;
using Flex.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace Flex.EFSql
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {

        }
        // 无参构造函数，用于设计时创建对象
        public SqlServerContext() : base()
        {
        }
        //此处用微软原生的控制台日志记录，如果使用NLog很可能数据库还没创建，造成记录日志到数据库性能下降（一直在尝试连接数据库，但是数据库还没创建）
        //此处使用静态实例，这样不会为每个上下文实例创建新的 ILoggerFactory 实例，这一点非常重要。 否则会导致内存泄漏和性能下降。
        //此处使用了Debug和console两种日志输出，会输出到控制台和调试窗口
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddDebug().AddConsole());
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            //if (!optionsBuilder.IsConfigured) // 只在未配置的情况下配置
            //{
            //    
            //    // 在这里添加其他的配置，例如
            //    //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=cms_core;uid=sa;pwd=123456;TrustServerCertificate=true;");
            //}
        }
        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated(); // 或者使用 Migrate() 方法
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = DAssemblyFinder.Instance.FindAll();
            assemblies.Foreach(assembly =>
            {
                assembly.ExportedTypes
                    .Where(type => type.IsClass && type != typeof(EntityContext) && typeof(EntityContext).IsAssignableFrom(type))
                    .Foreach(type =>
                    {
                        var method = modelBuilder
                        .GetType()
                        .GetMethods()
                        .FirstOrDefault(x => x.Name == "Entity");
                        if (method != null)
                        {
                            Console.WriteLine($"【{type.Name}】已加入自动DataSet");
                            method = method.MakeGenericMethod(type);
                            method.Invoke(modelBuilder, null);
                        }
                    });
                if (assembly.ExportedTypes
                    .Any(type => type.IsClass && type != typeof(BaseConfig) && typeof(BaseConfig).IsAssignableFrom(type)))
                {
                    Console.WriteLine($"【{assembly.GetName().Name}】已加入自动配置Config");
                    modelBuilder.ApplyConfigurationsFromAssembly(assembly);
                }
            });

            var usedb = "DataConfig:UseDb".Config(string.Empty) ?? "Sqlserver";
            if (usedb == "PostgreSQL")
            {
                // 将所有日期时间字段配置为使用UTC时间
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    foreach (var property in entityType.GetProperties())
                    {
                        if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        {
                            property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                                v => v.ToUniversalTime(),
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                        }
                    }
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
