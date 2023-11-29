using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class FlexCmsDbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_core_admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mutiloginccode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginIP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AllowMultiLogin = table.Column<bool>(type: "bit", nullable: false),
                    Islock = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginCount = table.Column<int>(type: "int", nullable: false),
                    FilterIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAvatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaltValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorCount = table.Column<int>(type: "int", nullable: false),
                    MaxErrorCount = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebsitePermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuPermissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPermission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlPermission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentID = table.Column<long>(type: "bigint", nullable: false),
                    ShowStatus = table.Column<bool>(type: "bit", nullable: false),
                    isMenu = table.Column<bool>(type: "bit", nullable: false),
                    IsControllerUrl = table.Column<bool>(type: "bit", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FontSort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolesName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebsitePermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuPermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataPermission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlPermission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RolesDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    AddUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true)
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
                    AddUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true)
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
                    AdminId = table.Column<long>(type: "bigint", nullable: false)
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
