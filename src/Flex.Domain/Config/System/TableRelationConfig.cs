using Flex.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Config.System
{
    public class TableRelationConfig : BaseConfig, IEntityTypeConfiguration<sysTableRelation>
    {
        public void Configure(EntityTypeBuilder<sysTableRelation> builder)
        {
            builder.ToTable("tbl_core_tablerelation");

            base.ConfigureIntId(builder);
        }
    }
}
