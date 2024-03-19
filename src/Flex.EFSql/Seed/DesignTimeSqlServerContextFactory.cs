using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Dm;

namespace Flex.EFSql;
public class DesignTimeSqlServerContextFactory : IDesignTimeDbContextFactory<SqlServerContext>
{
    public SqlServerContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        builder.UseNpgsql("Host=192.168.8.128;Port=15433;Database=postgres;Username=postgres;Password=password");
        //builder.UseDm("Server=127.0.0.1;Database=SYSDBA;UserId=SYSDBA;PWD=SYSDBA;Port=5236;encoding=utf-8;");
        //builder.UseSqlServer("Data Source=.;Initial Catalog=cms_core_copy;uid=sa;pwd=123456;TrustServerCertificate=true;");
        //builder.UseMySql("Server=192.168.8.128;Port=3366;Database=cms_core_copy;Uid=root;Pwd=123456;", new MySqlServerVersion(new Version("8.0.27")));

        return new SqlServerContext(builder.Options);
    }
}
