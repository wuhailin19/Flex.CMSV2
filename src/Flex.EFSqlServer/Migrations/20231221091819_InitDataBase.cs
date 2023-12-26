using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_core_admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mutiloginccode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastLoginIP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastLoginTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AllowMultiLogin = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Islock = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LoginCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FilterIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAvatar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UserSign = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SaltValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginLogString = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ErrorCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxErrorCount = table.Column<int>(type: "int", nullable: false, defaultValue: 10),
                    AddUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastEditUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WebsitePermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuPermissions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DataPermission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlPermission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastEditUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentID = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ShowStatus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    isMenu = table.Column<bool>(type: "bit", nullable: false),
                    IsControllerUrl = table.Column<bool>(type: "bit", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FontSort = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastEditUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_picture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Src = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AddUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastEditUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_picture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_pictureCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AddUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastEditUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_pictureCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolesName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WebsitePermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuPermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataPermission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlPermission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RolesDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    AddUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastEditUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_roleurl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxErrorCount = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    NeedActionPermission = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastEditUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_roleurl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_systemIndexSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index_System_Menu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index_Site_Menu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index_Shortcut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index_FileManage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminId = table.Column<long>(type: "bigint", nullable: false),
                    AddUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastEditUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_systemIndexSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_testTransaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_testTransaction", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "tbl_core_admin",
                columns: new[] { "Id", "Account", "AddTime", "AddUser", "AddUserName", "AllowMultiLogin", "FilterIp", "LastEditDate", "LastEditUser", "LastEditUserName", "LastLoginIP", "LastLoginTime", "LockTime", "LoginLogString", "Mutiloginccode", "Password", "RoleId", "RoleName", "SaltValue", "UserAvatar", "UserName", "UserSign", "Version" },
                values: new object[] { 1560206066204151804L, "webmaster", new DateTime(2023, 12, 21, 17, 18, 19, 399, DateTimeKind.Local).AddTicks(8755), 1560206066204151804L, "webmaster", true, null, null, 1560206066204151804L, "webmaster", "127.0.0.1", new DateTime(2023, 12, 21, 17, 18, 19, 399, DateTimeKind.Local).AddTicks(8755), null, null, "7675038.28325281", "5A72A8F355E9A88D03C30778C2770E27", 0, "超级管理员", "4ad9879fb285407f", null, "超级管理员", null, 0 });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[] { 1, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "根节点", 0, true, 1, 0, true });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 2, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-website", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "站点管理", 0, 1, true, 1, 0, true },
                    { 4, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-slider", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "columnCategory.aspx?SystemID=1", "栏目管理", 1, 153, true, 1, 0, false },
                    { 5, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-app", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "columnCategoryContent.aspx?SystemID=1", "内容管理", 2, 153, true, 1, 0, false },
                    { 6, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-wifi", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "webSites.aspx", "网站管理", 1, 1, true, 1, 0, false },
                    { 67, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-template-1", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "contentModel.aspx", "内容模型", 2, 1, true, 1, 0, true },
                    { 68, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-export", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "文件管理", 3, 1, true, 1, 0, true },
                    { 69, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "图片管理系统", 0, 68, true, 1, 0, true },
                    { 70, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-file-b", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "文件管理系统", 0, 68, true, 1, 0, true },
                    { 71, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-template-1", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "PictureCateGory.aspx", "图片分类管理", 0, 69, true, 1, 0, false },
                    { 72, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-picture", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "pictureManage.aspx", "图片管理", 0, 69, true, 1, 0, false },
                    { 73, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-group", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "成员管理", 4, 1, true, 1, 0, true },
                    { 129, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-component", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "fileCateGory.aspx", "文件分类管理", 0, 70, true, 1, 0, false },
                    { 130, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-file-b", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "fileManage.aspx", "文件管理", 0, 70, true, 1, 0, false },
                    { 131, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-friends", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "管理员管理", 0, 73, true, 1, 0, true }
                });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 132, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-username", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "会员系统管理", 0, 73, 1, 0, true },
                    { 135, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "adminManage.aspx", "账号管理（旧）", 0, 131, 1, 0, false }
                });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[] { 136, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconnavicon-jsgl", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "RolesPermission/Index", "角色管理", 0, 131, true, 1, 0, false });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 137, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "会员管理", 0, 132, 1, 0, false },
                    { 138, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "会员组管理", 0, 132, 1, 0, false },
                    { 139, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "会员参数配置", 0, 132, 1, 0, false }
                });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 140, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-chart", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "访问统计", 5, 1, true, 1, 0, true },
                    { 141, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconfangwenliang", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "visitsituation.aspx", "统计图表", 0, 140, true, 1, 0, false },
                    { 142, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "pageBox.aspx", "点击分布", 0, 140, true, 1, 0, false },
                    { 143, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "pageBoxs.aspx", "热力图", 0, 140, true, 1, 0, false },
                    { 144, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-set-fill", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "其他配置", 6, 1, true, 1, 0, true },
                    { 145, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "网站系统配置", 0, 144, true, 1, 0, true },
                    { 146, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "系统配置", 0, 145, true, 1, 0, true },
                    { 147, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconzidian", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "dictionaryManage.aspx?PID=0", "数据字典", 0, 146, true, 1, 0, false },
                    { 148, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "adminlog.aspx", "数据日志", 0, 170, true, 1, 0, false },
                    { 149, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconrizhi1", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "Menu/Index", "菜单管理", 0, 146, true, 1, 0, false }
                });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "StatusCode", "Version", "isMenu" },
                values: new object[] { 150, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "广告管理系统", 0, 144, 1, 0, false });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 151, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "广告分类管理", 0, 150, true, 1, 0, false },
                    { 152, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "广告管理", 0, 150, true, 1, 0, false },
                    { 153, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "后台管理系统", 0, 2, true, 1, 0, true }
                });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "StatusCode", "Version", "isMenu" },
                values: new object[] { 154, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "rolesManages.aspx", "角色管理（旧）", 0, 131, 1, 0, false });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 155, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-link", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "接口管理", 100, 1, true, 1, 0, true },
                    { 156, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "MenuApi/Index", "菜单接口", 0, 160, true, 1, 0, false },
                    { 157, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "MenuApi/DataApiPage", "数据接口", 0, 160, true, 1, 0, false },
                    { 158, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "SystemPermission/Index", "系统权限", 0, 146, true, 1, 0, false },
                    { 159, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "webConfig.aspx", "网站配置", 0, 146, true, 1, 0, false },
                    { 160, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "接口分类", 0, 155, true, 1, 0, true }
                });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 161, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "审核", 0, 144, 1, 0, false },
                    { 162, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-cart-simple", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "电商模块", 111, 1, 1, 0, true },
                    { 163, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "订单管理", 0, 162, 1, 0, false },
                    { 164, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-search", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "在线调查", 112, 1, 1, 0, true },
                    { 165, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "投票系统", 0, 164, 1, 0, false },
                    { 166, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-form", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "评论管理", 113, 1, 1, 0, false }
                });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 167, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconrizhi", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "SystemLog/Index", "系统日志", 0, 170, true, 1, 0, false },
                    { 168, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-username", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "Admin/Index", "账号管理", 0, 131, true, 1, 0, false },
                    { 169, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "MenuApi/Page", "视图管理", 0, 160, true, 1, 0, false },
                    { 170, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "日志模块", 0, 145, true, 1, 0, true },
                    { 171, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "testModel.aspx", "模型测试", 0, 67, true, 1, 0, false },
                    { 172, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "layui-icon-reply-fill", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "Admin/SendEmail", "信箱管理", 0, 1, true, 1, 0, false }
                });

            migrationBuilder.InsertData(
                table: "tbl_core_systemIndexSet",
                columns: new[] { "Id", "AddUser", "AddUserName", "AdminId", "Index_FileManage", "LastEditDate", "LastEditUser", "LastEditUserName", "Index_Shortcut", "Index_Site_Menu", "StatusCode", "Index_System_Menu", "Version" },
                values: new object[] { 1, 1560206066204151804L, "webmaster", 1560206066204151804L, "5,6", null, null, null, "", "171", 1, "5,158,167,172", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_core_admin");

            migrationBuilder.DropTable(
                name: "tbl_core_group");

            migrationBuilder.DropTable(
                name: "tbl_core_menu");

            migrationBuilder.DropTable(
                name: "tbl_core_picture");

            migrationBuilder.DropTable(
                name: "tbl_core_pictureCategory");

            migrationBuilder.DropTable(
                name: "tbl_core_role");

            migrationBuilder.DropTable(
                name: "tbl_core_roleurl");

            migrationBuilder.DropTable(
                name: "tbl_core_systemIndexSet");

            migrationBuilder.DropTable(
                name: "tbl_core_testTransaction");
        }
    }
}
