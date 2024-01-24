using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class FieldConfig : BaseConfig, IEntityTypeConfiguration<sysField>
    {
        public void Configure(EntityTypeBuilder<sysField> builder)
        {
            builder.ToTable("tbl_core_field");
            builder.Property(m => m.Name).HasMaxLength(50).IsRequired();
            builder.Property(m => m.FieldName).HasMaxLength(50).IsRequired();
            builder.Property(m => m.FieldDescription).HasMaxLength(500);
            builder.Property(m => m.Validation).HasMaxLength(500);
            builder.Property(m => m.FieldAttritude).HasMaxLength(500);
            builder.Property(m => m.ApiName).HasMaxLength(50);
            builder.Property(m => m.FieldType).HasMaxLength(50);
            builder.Property(m => m.IsApiField).HasDefaultValue(false);
            builder.Property(m => m.IsSearch).HasDefaultValue(false);
            builder.Property(m => m.ShowInTable).HasDefaultValue(false);
            builder.Property(m => m.OrderId).HasDefaultValue(0);
            builder.Property(m => m.ModelId).HasDefaultValue(0).IsRequired();
            base.ConfigureStringId(builder);
        }
    }
}
