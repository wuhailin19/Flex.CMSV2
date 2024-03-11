using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Flex.EFSql;
public class DesignTimeSqlServerContextFactory : IDesignTimeDbContextFactory<SqlServerContext>
{
    public SqlServerContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        //builder.UseSqlServer("Data Source=.;Initial Catalog=cms_core_copy;uid=sa;pwd=123456;TrustServerCertificate=true;");
        builder.UseMySql("Server=192.168.8.128;Port=3366;Database=cms_core_copy;Uid=root;Pwd=123456;", new MySqlServerVersion(new Version("8.0.27")));

        return new SqlServerContext(builder.Options);
    }
}
