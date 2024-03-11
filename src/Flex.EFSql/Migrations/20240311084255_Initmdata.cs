using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flex.EFSql.Migrations
{
    /// <inheritdoc />
    public partial class Initmdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Account = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mutiloginccode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentLoginIP = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentLoginTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LockTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AllowMultiLogin = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: true),
                    Islock = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FilterIp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAvatar = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserSign = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaltValue = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginLogString = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxErrorCount = table.Column<int>(type: "int", nullable: false, defaultValue: 10),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 3, DateTimeKind.Local).AddTicks(6953)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_admin", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_column",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiteId = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColumnImage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModelId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    ExtensionModelId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    ReviewMode = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    ParentId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsShow = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ColumnUrl = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoTitle = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoKeyWord = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoDescription = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 4, DateTimeKind.Local).AddTicks(2282)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_column", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_contentmodel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TableName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormHtmlString = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 4, DateTimeKind.Local).AddTicks(5824)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_contentmodel", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_field",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Validation = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldAttritude = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApiName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsApiField = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: false),
                    IsSearch = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: false),
                    ShowInTable = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: false),
                    ModelId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 5, DateTimeKind.Local).AddTicks(2003)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_field", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GroupName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WebsitePermissions = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuPermissions = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataPermission = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrlPermission = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupDesc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 5, DateTimeKind.Local).AddTicks(5228)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_group", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentID = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ShowStatus = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: true),
                    isMenu = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsControllerUrl = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LinkUrl = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FontSort = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 6, DateTimeKind.Local).AddTicks(5460)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_menu", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MsgContent = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RabbitMqQueueName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ToUserId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ToRoleId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FromPathId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ToPathId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TableName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    FlowId = table.Column<int>(type: "int", nullable: false),
                    IsStart = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsEnd = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MsgGroupId = table.Column<long>(type: "bigint", nullable: false),
                    RecieveId = table.Column<int>(type: "int", nullable: false),
                    MessageCate = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 6, DateTimeKind.Local).AddTicks(8494)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_message", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_picture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Src = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryID = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 7, DateTimeKind.Local).AddTicks(7582)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_picture", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_pictureCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 7, DateTimeKind.Local).AddTicks(3329)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_pictureCategory", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RolesName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WebsitePermissions = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuPermissions = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataPermission = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrlPermission = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RolesDesc = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 8, DateTimeKind.Local).AddTicks(1418)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_role", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_roleurl",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReturnContent = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxErrorCount = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NeedActionPermission = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 8, DateTimeKind.Local).AddTicks(4820)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_roleurl", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_systemIndexSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Index_System_Menu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Index_Site_Menu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Index_Shortcut = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Index_FileManage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdminId = table.Column<long>(type: "bigint", nullable: false),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 5, DateTimeKind.Local).AddTicks(9736)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_systemIndexSet", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_testTransaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_testTransaction", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_workflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkFlowContent = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepDesign = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    actDesign = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    actionString = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Introduction = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 9, DateTimeKind.Local).AddTicks(8748)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_workflow", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_workflowaction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    actionPathId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    actionName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    flowId = table.Column<int>(type: "int", nullable: false),
                    conjunctManFlag = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    directMode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orgBossMode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepFromId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepToId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    actionFromName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    actionToName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepToCate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 9, DateTimeKind.Local).AddTicks(5422)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_workflowaction", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_core_workflowstep",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepPathId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    flowId = table.Column<int>(type: "int", nullable: false),
                    avoidFlag = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orgMode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepMan = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepOrg = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepRole = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stepCate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isStart = table.Column<int>(type: "int", nullable: false),
                    AddUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddUser = table.Column<long>(type: "bigint", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 3, 11, 16, 42, 55, 10, DateTimeKind.Local).AddTicks(2089)),
                    LastEditUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEditUser = table.Column<long>(type: "bigint", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_workflowstep", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "tbl_core_admin",
                columns: new[] { "Id", "Account", "AddTime", "AddUser", "AddUserName", "AllowMultiLogin", "CurrentLoginIP", "CurrentLoginTime", "FilterIp", "LastEditDate", "LastEditUser", "LastEditUserName", "LockTime", "LoginLogString", "Mutiloginccode", "Password", "RoleId", "RoleName", "SaltValue", "UserAvatar", "UserName", "UserSign", "Version" },
                values: new object[] { 1560206066204151804L, "webmaster", new DateTime(2024, 3, 11, 16, 42, 55, 3, DateTimeKind.Local).AddTicks(6528), 1560206066204151804L, "webmaster", true, "127.0.0.1", new DateTime(2024, 3, 11, 16, 42, 55, 3, DateTimeKind.Local).AddTicks(6528), null, null, 1560206066204151804L, "webmaster", null, null, "7675038.28325281", "4539390B11E07C307C7A2C4224DF3109", 0, "超级管理员", "4ad9879fb285407f", null, "超级管理员", null, 0 });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[] { 1, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "fontClass", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "根节点", 0, true, 1, 0, true });

            migrationBuilder.InsertData(
                table: "tbl_core_menu",
                columns: new[] { "Id", "AddTime", "AddUser", "AddUserName", "FontSort", "Icode", "IsControllerUrl", "LastEditDate", "LastEditUser", "LastEditUserName", "Level", "LinkUrl", "Name", "OrderId", "ParentID", "ShowStatus", "StatusCode", "Version", "isMenu" },
                values: new object[,]
                {
                    { 2, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-read", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "内容管理", 0, 1, true, 1, 0, true },
                    { 4, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-app", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "/System/columnCategory/Index", "栏目管理", 1, 153, true, 1, 0, false },
                    { 5, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-read", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "/System/columnContent/Manage", "内容管理", 2, 153, true, 1, 0, false },
                    { 6, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-website", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "webSites.aspx", "站点管理", 1, 1, true, 1, 0, false },
                    { 67, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-template-1", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "内容模型", 2, 1, true, 1, 0, true },
                    { 69, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-picture", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "图片管理", 1, 1, true, 1, 0, false },
                    { 73, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-group", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "成员管理", 4, 1, true, 1, 0, true },
                    { 131, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-friends", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "管理员管理", 0, 73, true, 1, 0, true },
                    { 132, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-username", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "会员系统管理", 0, 73, true, 1, 0, true },
                    { 136, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconnavicon-jsgl", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "RolesPermission/Index", "角色管理", 0, 131, true, 1, 0, false },
                    { 137, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "会员管理", 0, 132, true, 1, 0, false },
                    { 138, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "会员组管理", 0, 132, true, 1, 0, false },
                    { 139, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "会员参数配置", 0, 132, true, 1, 0, false },
                    { 144, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-set-fill", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "其他配置", 6, 1, true, 1, 0, true },
                    { 145, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "网站系统配置", 0, 144, true, 1, 0, true },
                    { 146, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "系统配置", 0, 145, true, 1, 0, true },
                    { 147, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconzidian", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "dictionaryManage.aspx?PID=0", "数据字典", 0, 146, true, 1, 0, false },
                    { 148, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "adminlog.aspx", "数据日志", 0, 170, true, 1, 0, false },
                    { 149, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconrizhi1", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "Menu/Index", "菜单管理", 0, 146, true, 1, 0, false },
                    { 153, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "后台管理系统", 0, 2, true, 1, 0, true },
                    { 155, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-link", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "接口管理", 100, 1, true, 1, 0, true },
                    { 156, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "MenuApi/Index", "菜单接口", 0, 160, true, 1, 0, false },
                    { 157, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "MenuApi/DataApiPage", "数据接口", 0, 160, true, 1, 0, false },
                    { 159, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "webConfig.aspx", "网站配置", 0, 146, true, 1, 0, false },
                    { 160, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "接口分类", 0, 155, true, 1, 0, true },
                    { 161, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-chart-screen", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "审核", 5, 1, true, 1, 0, false },
                    { 162, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-cart-simple", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "电商模块", 111, 1, true, 1, 0, true },
                    { 163, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "订单管理", 0, 162, true, 1, 0, false },
                    { 164, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-search", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "在线调查", 112, 1, true, 1, 0, true },
                    { 165, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", true, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "投票系统", 0, 164, true, 1, 0, false },
                    { 166, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-form", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "评论管理", 113, 1, true, 1, 0, false },
                    { 167, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "iconfont", "iconrizhi", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "SystemLog/Index", "系统日志", 0, 170, true, 1, 0, false },
                    { 168, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-username", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "Admin/Index", "账号管理", 0, 131, true, 1, 0, false },
                    { 169, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "MenuApi/Page", "视图管理", 0, 160, true, 1, 0, false },
                    { 170, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "", "日志模块", 0, 145, true, 1, 0, true },
                    { 171, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "/system/ContentModel/Index", "模型管理", 0, 67, true, 1, 0, false },
                    { 172, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-reply-fill", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "Admin/SendEmail", "信箱管理", 7, 1, true, 1, 0, false },
                    { 174, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "/system/WorkFlow/Index", "审核流程图", 0, 161, true, 1, 0, false },
                    { 175, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", "layui-icon", "layui-icon-export", false, new DateTime(2022, 8, 25, 16, 31, 4, 0, DateTimeKind.Unspecified), 1560206066204151804L, "webmaster", 0, "/system/FileManage/Index", "文件管理", 1, 1, true, 1, 0, false }
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
                name: "tbl_core_column");

            migrationBuilder.DropTable(
                name: "tbl_core_contentmodel");

            migrationBuilder.DropTable(
                name: "tbl_core_field");

            migrationBuilder.DropTable(
                name: "tbl_core_group");

            migrationBuilder.DropTable(
                name: "tbl_core_menu");

            migrationBuilder.DropTable(
                name: "tbl_core_message");

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

            migrationBuilder.DropTable(
                name: "tbl_core_workflow");

            migrationBuilder.DropTable(
                name: "tbl_core_workflowaction");

            migrationBuilder.DropTable(
                name: "tbl_core_workflowstep");
        }
    }
}
