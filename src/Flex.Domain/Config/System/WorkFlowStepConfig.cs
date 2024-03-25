using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class WorkFlowStepConfig : BaseConfig, IEntityTypeConfiguration<sysWorkFlowStep>
    {
        public void Configure(EntityTypeBuilder<sysWorkFlowStep> builder)
        {
            builder.ToTable("tbl_core_workflowstep");

            base.ConfigureStringId(builder);
        }
    }
}
