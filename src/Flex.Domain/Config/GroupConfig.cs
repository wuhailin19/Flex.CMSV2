using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class GroupConfig : BaseConfig, IEntityTypeConfiguration<SysGroup>
    {
        public void Configure(EntityTypeBuilder<SysGroup> builder)
        {
            base.ConfigureLongId(builder);
            builder.Property(m=>m.GroupName).IsRequired().HasMaxLength(50);
            builder.ToTable("tbl_core_group");
        }
    }
}
