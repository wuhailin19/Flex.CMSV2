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

            builder.ToTable("tbl_core_role");
        }
    }
}
