﻿using Flex.Core;
using Flex.Core.Extensions;
using Flex.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Localization;

namespace Flex.Domain.Config
{
    public class AdminConfig : BaseConfig, IEntityTypeConfiguration<SysAdmin>
    {
		public void Configure(EntityTypeBuilder<SysAdmin> builder)
        {
            //Console.WriteLine("Configuring SysAdmin entity...");
            builder.ToTable("tbl_core_admin");
            builder.Ignore(m => m.IsSystem);
            
            builder.Property(m => m.Account).HasMaxLength(100).IsRequired();
            builder.Property(m => m.UserName).HasMaxLength(100);
            builder.Property(m => m.Password).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Mutiloginccode).HasMaxLength(20);
            builder.Property(m => m.CurrentLoginIP).HasMaxLength(20);
            builder.Property(m => m.RoleName).HasMaxLength(20);
            builder.Property(m => m.UserSign).HasMaxLength(200);
            builder.Property(m => m.UserAvatar).HasMaxLength(200);
            builder.Property(m => m.LoginLogString).HasMaxLength(200);
            builder.Property(m => m.LoginCount).HasDefaultValue(0);
            builder.Property(m => m.MaxErrorCount).HasDefaultValue(5);
            builder.Property(m => m.ErrorCount).HasDefaultValue(0);
            builder.Property(m => m.Islock).HasDefaultValue(false);
            builder.Property(m => m.AllowMultiLogin).HasDefaultValue(true);
            builder.Property(m => m.StatusCode).HasDefaultValue(StatusCode.Enable);
            builder.Property(m => m.Version).HasDefaultValue(0);

            ConfigureSeedData(builder);
            base.ConfigureLongId(builder);

        }
        private void ConfigureSeedData(EntityTypeBuilder<SysAdmin> builder)
        {
            DateTime dateTime = DateTime.Now;
            builder.HasData(
                new SysAdmin
                {
                    Id = 1560206066204151804,
                    Account = "webmaster",
                    UserName = "超级管理员",
                    Password = "4539390B11E07C307C7A2C4224DF3109",
                    CurrentLoginIP = "127.0.0.1",
                    CurrentLoginTime = dateTime,
                    AllowMultiLogin = true,
                    SaltValue= "4ad9879fb285407f",
                    RoleId = 0,
                    RoleName= "超级管理员",
                    LoginCount = 0,
                    Islock = false,
                    Mutiloginccode= "7675038.28325281",
                    AddTime= dateTime,
                    AddUserName= "webmaster",
                    LastEditUserName= "webmaster",
                    LockTime=null,
                    AddUser= 1560206066204151804,
                    LastEditUser = 1560206066204151804
                }
                ) ;
        }
    }
}
