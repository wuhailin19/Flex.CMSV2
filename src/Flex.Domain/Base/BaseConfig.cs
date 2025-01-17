﻿using Flex.Core.Timing;
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
            builder.Property(m => m.AddTime).HasDefaultValue(Clock.Now);
            builder.Property(m => m.AddUserName).HasMaxLength(200);
            builder.Property(m => m.LastEditUserName).HasMaxLength(200);
            builder.Property(m => m.StatusCode).HasDefaultValue(StatusCode.Enable);
            builder.Property(m => m.Version).HasDefaultValue(0);
            builder.Property(m => m.Id).ValueGeneratedNever(); // ID 不自增
            builder.HasQueryFilter(m => m.StatusCode != StatusCode.Deleted);
            builder.HasKey(c => c.Id);
        }
        public virtual void ConfigureIntId<T>(EntityTypeBuilder<T> builder) where T : BaseIntEntity
        {
            builder.Property(m => m.AddTime).HasDefaultValue(Clock.Now);
            builder.Property(m => m.AddUserName).HasMaxLength(200);
            builder.Property(m => m.LastEditUserName).HasMaxLength(200);
            builder.Property(m => m.StatusCode).HasDefaultValue(StatusCode.Enable);
            builder.Property(m => m.Version).HasDefaultValue(0);
            builder.HasQueryFilter(m => m.StatusCode != StatusCode.Deleted);
            builder.HasKey(c => c.Id);
        }
        public virtual void ConfigureStringId<T>(EntityTypeBuilder<T> builder) where T : BaseEntity
        {
            builder.Property(m => m.AddTime).HasDefaultValue(Clock.Now);
            builder.Property(m => m.AddUserName).HasMaxLength(200);
            builder.Property(m => m.LastEditUserName).HasMaxLength(200);
            builder.Property(m => m.StatusCode).HasDefaultValue(StatusCode.Enable);
            builder.Property(m => m.Version).HasDefaultValue(0);
            builder.Property(m => m.Id).ValueGeneratedNever(); // ID 不自增
            builder.HasQueryFilter(m => m.StatusCode != StatusCode.Deleted);
            builder.HasKey(c => c.Id);
        }
    }
}
