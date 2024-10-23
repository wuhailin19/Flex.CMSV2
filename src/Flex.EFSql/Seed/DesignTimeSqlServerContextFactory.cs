using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Dm;

namespace Flex.EFSql;
public class DesignTimeSqlServerContextFactory : IDesignTimeDbContextFactory<EfCoreDBContext>
{
    public EfCoreDBContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<EfCoreDBContext>();
        //builder.UseNpgsql("Host=192.168.8.128;Port=15433;Database=cms_core_copy;Username=postgres;Password=password");
        //builder.UseDm("Server=118.31.58.175;UserId=SYSDBA;PWD=SYSDBA001;Database=cms_core_copy;Port=5236;encoding=utf-8;");
        //builder.UseSqlServer("Data Source=.;Initial Catalog=cms_core_copy;uid=sa;pwd=123456;TrustServerCertificate=true;");
        //builder.UseDm("Server=127.0.0.1;UserId=CMSADMIN;PWD=SYSDBA;Database=CMSADMIN;Port=5236;encoding=utf-8;");
        builder.UseMySql("Server=211.149.184.98;Port=3306;Database=cms_core;Uid=newadmin;Pwd=tJS:eelgt1*o;", new MySqlServerVersion(new Version("8.0.27")));

        return new EfCoreDBContext(builder.Options);
    }
}
