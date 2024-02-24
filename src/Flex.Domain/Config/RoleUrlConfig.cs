using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class RoleUrlConfig : BaseConfig, IEntityTypeConfiguration<SysRoleUrl>
    {
        public void Configure(EntityTypeBuilder<SysRoleUrl> builder)
        {
            builder.ToTable("tbl_core_roleurl");

            base.ConfigureStringId(builder);
        }
    }
}
