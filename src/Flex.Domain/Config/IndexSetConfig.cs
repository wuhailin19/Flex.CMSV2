using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class IndexSetConfig : BaseConfig, IEntityTypeConfiguration<SysIndexSet>
    {
        public void Configure(EntityTypeBuilder<SysIndexSet> builder)
        {
            builder.ToTable("tbl_core_systemIndexSet");
            builder.HasKey(m=>m.Id);
        }
    }
}
