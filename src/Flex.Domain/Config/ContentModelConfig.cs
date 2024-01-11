using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class ContentModelConfig : BaseConfig, IEntityTypeConfiguration<SysContentModel>
    {
        public void Configure(EntityTypeBuilder<SysContentModel> builder)
        {
            builder.ToTable("tbl_core_contentmodel");
            builder.Property(m => m.Name).HasMaxLength(50).IsRequired();
            builder.Property(m => m.Description).HasMaxLength(100);
            builder.Property(m => m.TableName).HasMaxLength(50).IsRequired();
            base.ConfigureIntId(builder);
        }
    }
}
