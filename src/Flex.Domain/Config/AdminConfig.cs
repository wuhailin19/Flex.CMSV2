using Flex.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class AdminConfig : BaseConfig,IEntityTypeConfiguration<SysAdmin>
    {
        public void Configure(EntityTypeBuilder<SysAdmin> builder)
        {
            Console.WriteLine("Configuring SysAdmin entity...");
            builder.ToTable("tbl_core_admin");
            builder.Ignore(m=>m.IsSystem);
            base.ConfigureLongId(builder);

        }
    }
}
