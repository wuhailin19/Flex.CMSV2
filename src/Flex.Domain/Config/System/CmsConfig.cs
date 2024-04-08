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
    public class CmsConfig : BaseConfig, IEntityTypeConfiguration<sysCmsConfig>
    {
        public void Configure(EntityTypeBuilder<sysCmsConfig> builder)
        {
            builder.ToTable("tbl_core_cmsconfig");
            base.ConfigureIntId(builder);
        }
    }
}
