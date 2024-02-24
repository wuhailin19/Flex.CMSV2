using Flex.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Config
{
    internal class MessageConfig : BaseConfig, IEntityTypeConfiguration<sysMessage>
    {
        public void Configure(EntityTypeBuilder<sysMessage> builder)
        {
            builder.ToTable("tbl_core_message");
            base.ConfigureIntId(builder);
        }
    }
}
