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
            builder.Property(m => m.SiteId).HasDefaultValue(1);
            builder.Property(m => m.ParentId).HasDefaultValue(0);
            builder.Property(m => m.SeoTitle).HasMaxLength(250);
            builder.Property(m => m.SeoKeyWord).HasMaxLength(500);
            builder.Property(m => m.SeoDescription).HasMaxLength(2000);
            base.ConfigureIntId(builder);
        }
    }
}
