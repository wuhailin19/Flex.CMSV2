using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Base
{
    /// <summary>
    /// 自动加入EFSqlServerConfig
    /// </summary>
    public abstract class BaseConfig
    {
        public virtual void Configure<T>(EntityTypeBuilder<T> builder) where T : BaseEntity
        {
            builder.HasKey(c => c.Id);
            //builder.HasQueryFilter(b => b.StatusCode != StatusCode.Deleted);
        }
        public virtual void ConfigureLongId<T>(EntityTypeBuilder<T> builder) where T : BaseLongEntity
        {
            builder.Property(m => m.AddUserName).HasMaxLength(200);
            builder.Property(m => m.LastEditUserName).HasMaxLength(200);
            builder.HasKey(c => c.Id);
            //builder.HasQueryFilter(b => b.StatusCode != StatusCode.Deleted);
        }
        public virtual void ConfigureIntId<T>(EntityTypeBuilder<T> builder) where T : BaseIntEntity
        {

            builder.Property(m => m.AddUserName).HasMaxLength(200);
            builder.Property(m => m.LastEditUserName).HasMaxLength(200);
            builder.HasKey(c => c.Id);
            //builder.Property(c => c.Id).ValueGeneratedNever();
            //builder.HasQueryFilter(b => b.StatusCode != StatusCode.Deleted);
        }
    }
}
