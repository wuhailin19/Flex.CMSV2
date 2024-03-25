using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class TranscationConfig : BaseConfig, IEntityTypeConfiguration<TestTranscation>
    {
        public void Configure(EntityTypeBuilder<TestTranscation> builder)
        {
            builder.ToTable("tbl_core_testTransaction");
            builder.HasKey(m => m.Id);
        }
    }
}
