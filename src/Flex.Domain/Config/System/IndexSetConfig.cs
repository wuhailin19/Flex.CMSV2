using Flex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flex.Domain.Config
{
    public class IndexSetConfig : BaseConfig, IEntityTypeConfiguration<SysIndexSet>
    {
        public void Configure(EntityTypeBuilder<SysIndexSet> builder)
        {
            builder.ToTable("tbl_core_systemIndexSet");

            ConfigureSeedData(builder);
            base.ConfigureIntId(builder);
        }
        private void ConfigureSeedData(EntityTypeBuilder<SysIndexSet> builder)
        {
            builder.HasData(
                new SysIndexSet
                {
                    Id = 1,
                    SystemMenu = "5,158,167,172",
                    SiteMenu = "171",
                    FileManage = "5,6",
                    Shortcut = string.Empty,
                    AddUser= 1560206066204151804,
                    AddUserName="webmaster",
                    StatusCode=StatusCode.Enable,
                    AdminId = 1560206066204151804
                }
                );
        }
    }
}
