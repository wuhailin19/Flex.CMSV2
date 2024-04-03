using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Config
{
    public class PictureCategoryConfig : BaseConfig, IEntityTypeConfiguration<sysPictureCategory>
    {
        public void Configure(EntityTypeBuilder<sysPictureCategory> builder)
        {
            builder.ToTable("tbl_core_pictureCategory");
            builder.Property(m => m.Name).HasMaxLength(255);
            builder.Property(m => m.Description).HasMaxLength(255);
            builder.Property(m=>m.OrderId).HasDefaultValue(0);
            base.ConfigureIntId(builder);
        }
    }
}
