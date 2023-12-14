using Microsoft.EntityFrameworkCore;
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
     
        public virtual void ConfigureLongId<T>(EntityTypeBuilder<T> builder) where T : BaseLongEntity
        {
            builder.Property(m => m.AddTime).HasDefaultValueSql("GETDATE()");
            builder.Property(m => m.AddUserName).HasMaxLength(200);
            builder.Property(m => m.LastEditUserName).HasMaxLength(200);
            builder.HasKey(c => c.Id);
        }
        public virtual void ConfigureIntId<T>(EntityTypeBuilder<T> builder) where T : BaseIntEntity
        {
            builder.Property(m => m.AddTime).HasDefaultValueSql("GETDATE()");
            builder.Property(m => m.AddUserName).HasMaxLength(200);
            builder.Property(m => m.LastEditUserName).HasMaxLength(200);
            builder.HasKey(c => c.Id);
        }
    }
}
