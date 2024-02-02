using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class WorkFlowActionConfig : BaseConfig, IEntityTypeConfiguration<sysWorkFlowAction>
    {
        public void Configure(EntityTypeBuilder<sysWorkFlowAction> builder)
        {
            builder.ToTable("tbl_core_workflowaction");

            base.ConfigureStringId(builder);
        }
    }
}
