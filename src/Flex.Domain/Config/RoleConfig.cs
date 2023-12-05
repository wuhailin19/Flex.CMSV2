using Flex.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class RoleConfig : BaseConfig, IEntityTypeConfiguration<SysRole>
    {
        public void Configure(EntityTypeBuilder<SysRole> builder)
        {
            base.ConfigureLongId(builder);
            builder.Property(m => m.RolesName).HasMaxLength(20).IsRequired();
            builder.Property(m => m.RolesDesc).HasMaxLength(20);
            builder.ToTable("tbl_core_role");
        }
    }
}
