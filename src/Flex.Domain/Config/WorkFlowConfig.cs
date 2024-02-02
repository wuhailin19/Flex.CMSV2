using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class WorkFlowConfig : BaseConfig, IEntityTypeConfiguration<sysWorkFlow>
    {
        public void Configure(EntityTypeBuilder<sysWorkFlow> builder)
        {
            builder.ToTable("tbl_core_workflow");

            base.ConfigureIntId(builder);
        }
    }
}
