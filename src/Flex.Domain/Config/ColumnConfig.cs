using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class ColumnConfig : BaseConfig, IEntityTypeConfiguration<SysColumn>
    {
        public void Configure(EntityTypeBuilder<SysColumn> builder)
        {
            builder.ToTable("tbl_core_column");
            builder.Property(m => m.Name).HasMaxLength(50).IsRequired();
            builder.Property(m => m.ColumnUrl).HasMaxLength(50);
            base.ConfigureIntId(builder);
        }
    }
}
