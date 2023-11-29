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
            base.ConfigureIntId(builder);

        }
    }
}
