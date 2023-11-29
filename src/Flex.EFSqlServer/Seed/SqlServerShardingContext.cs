using Flex.Core;
using Flex.Core.Extensions;
using Flex.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShardingCore.Core.VirtualRoutes.TableRoutes.RouteTails.Abstractions;
using ShardingCore.Sharding;
using ShardingCore.Sharding.Abstractions;

namespace Flex.EFSqlServer
{
    public class SqlServerShardingContext : AbstractShardingDbContext, IShardingTableDbContext
    {
        public SqlServerShardingContext(DbContextOptions<SqlServerShardingContext> options) : base(options)
        {
        }
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddDebug().AddConsole());

        public IRouteTail RouteTail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
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
            base.OnModelCreating(modelBuilder);
        }
    }
}
