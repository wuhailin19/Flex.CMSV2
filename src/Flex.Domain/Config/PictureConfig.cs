using Flex.Domain.Entities;
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
    public class PictureConfig : BaseConfig, IEntityTypeConfiguration<sysPicture>
    {
        public void Configure(EntityTypeBuilder<sysPicture> builder)
        {
            builder.ToTable("tbl_core_picture");
            builder.Property(m=>m.Name).HasMaxLength(255);
            builder.Property(m=>m.Description).HasMaxLength(255);
            builder.Property(m=>m.Src).HasMaxLength(255);
            builder.Property(m=>m.CategoryID).HasDefaultValue(0);
            base.ConfigureIntId(builder);
        }
    }
}
