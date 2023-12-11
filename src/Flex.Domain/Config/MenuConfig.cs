using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class MenuConfig : BaseConfig, IEntityTypeConfiguration<SysMenu>
    {
        public void Configure(EntityTypeBuilder<SysMenu> builder)
        {
            builder.ToTable("tbl_core_menu");
            builder.Property(m => m.Name).HasMaxLength(50);
            builder.Property(m => m.Icode).HasMaxLength(50);
            builder.Property(m => m.ParentID).HasDefaultValue(0);
            builder.Property(m => m.Version).HasDefaultValue(0);
            base.ConfigureIntId(builder);

        }
    }
}
