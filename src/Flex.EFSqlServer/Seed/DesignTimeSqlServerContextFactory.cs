using Flex.EFSqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Flex.EFSqlServer;
public class DesignTimeSqlServerContextFactory : IDesignTimeDbContextFactory<SqlServerContext>
{
    public SqlServerContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        builder.UseSqlServer("Data Source=.;Initial Catalog=cms_core;uid=sa;pwd=123456;TrustServerCertificate=true;");

        return new SqlServerContext(builder.Options);
    }
}
