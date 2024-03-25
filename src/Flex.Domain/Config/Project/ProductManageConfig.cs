using Flex.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class ProductManageConfig : BaseConfig, IEntityTypeConfiguration<projectProductManage>
    {
        public void Configure(EntityTypeBuilder<projectProductManage> builder)
        {
            builder.ToTable("tbl_normal_productmanage");

            base.ConfigureIntId(builder);
        }
    }
}
