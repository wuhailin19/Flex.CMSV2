using Flex.Domain.Entities.Normal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class ProductDetailConfig : BaseConfig, IEntityTypeConfiguration<norProductDetail>
    {
        public void Configure(EntityTypeBuilder<norProductDetail> builder)
        {
            builder.ToTable("tbl_normal_productdetail");

            base.ConfigureIntId(builder);
        }
    }
}
