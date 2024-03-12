using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSql.Migrations
{
    public partial class Initmenudata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_core_admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "BIGINT", nullable: false),
                    Account = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Mutiloginccode = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    CurrentLoginIP = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    CurrentLoginTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    LockTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    AllowMultiLogin = table.Column<bool>(type: "BIT", nullable: true, defaultValue: true),
                    Islock = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    RoleId = table.Column<int>(type: "INT", nullable: false),
                    RoleName = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    LoginCount = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    FilterIp = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    UserAvatar = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    UserSign = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    SaltValue = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    LoginLogString = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    ErrorCount = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    MaxErrorCount = table.Column<int>(type: "INT", nullable: false, defaultValue: 10),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 304, DateTimeKind.Local).AddTicks(4187)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_column",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    SiteId = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ColumnImage = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    ModelId = table.Column<int>(type: "INT", nullable: true, defaultValue: 0),
                    ExtensionModelId = table.Column<int>(type: "INT", nullable: true, defaultValue: 0),
                    ReviewMode = table.Column<int>(type: "INT", nullable: true, defaultValue: 0),
                    ParentId = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    IsShow = table.Column<bool>(type: "BIT", nullable: false),
                    ColumnUrl = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    SeoTitle = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: true),
                    SeoKeyWord = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    SeoDescription = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    OrderId = table.Column<int>(type: "INT", nullable: false),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 304, DateTimeKind.Local).AddTicks(6815)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_column", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_contentmodel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    TableName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    FormHtmlString = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 304, DateTimeKind.Local).AddTicks(8325)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_contentmodel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_field",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    FieldName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    FieldDescription = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    FieldType = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    Validation = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    FieldAttritude = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    ApiName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    IsApiField = table.Column<bool>(type: "BIT", nullable: true, defaultValue: false),
                    IsSearch = table.Column<bool>(type: "BIT", nullable: true, defaultValue: false),
                    ShowInTable = table.Column<bool>(type: "BIT", nullable: true, defaultValue: false),
                    ModelId = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 305, DateTimeKind.Local).AddTicks(2393)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_field", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "BIGINT", nullable: false),
                    GroupName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    WebsitePermissions = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    MenuPermissions = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: false),
                    DataPermission = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    UrlPermission = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    GroupDesc = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 305, DateTimeKind.Local).AddTicks(4186)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Icode = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ParentID = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    ShowStatus = table.Column<bool>(type: "BIT", nullable: true, defaultValue: true),
                    isMenu = table.Column<bool>(type: "BIT", nullable: false),
                    IsControllerUrl = table.Column<bool>(type: "BIT", nullable: false),
                    LinkUrl = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    FontSort = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    Level = table.Column<int>(type: "INT", nullable: false),
                    OrderId = table.Column<int>(type: "INT", nullable: false),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 305, DateTimeKind.Local).AddTicks(9225)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    Title = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    MsgContent = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    RabbitMqQueueName = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    ToUserId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    ToRoleId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    FromPathId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    ToPathId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    IsRead = table.Column<bool>(type: "BIT", nullable: false),
                    TableName = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    ContentId = table.Column<int>(type: "INT", nullable: false),
                    ParentId = table.Column<int>(type: "INT", nullable: false),
                    FlowId = table.Column<int>(type: "INT", nullable: false),
                    IsStart = table.Column<bool>(type: "BIT", nullable: false),
                    IsEnd = table.Column<bool>(type: "BIT", nullable: false),
                    MsgGroupId = table.Column<long>(type: "BIGINT", nullable: false),
                    RecieveId = table.Column<int>(type: "INT", nullable: false),
                    MessageCate = table.Column<int>(type: "INT", nullable: false),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 306, DateTimeKind.Local).AddTicks(574)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_picture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Src = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    CategoryID = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 306, DateTimeKind.Local).AddTicks(4301)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_picture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_pictureCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    OrderId = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 306, DateTimeKind.Local).AddTicks(2406)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_pictureCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    RolesName = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    WebsitePermissions = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    MenuPermissions = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    DataPermission = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    UrlPermission = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    RolesDesc = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    GroupId = table.Column<long>(type: "BIGINT", nullable: true),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 306, DateTimeKind.Local).AddTicks(5813)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_roleurl",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    Url = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    ReturnContent = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    RequestType = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    MaxErrorCount = table.Column<int>(type: "INT", nullable: false),
                    Category = table.Column<int>(type: "INT", nullable: false),
                    ParentId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    NeedActionPermission = table.Column<bool>(type: "BIT", nullable: false),
                    OrderId = table.Column<int>(type: "INT", nullable: false),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 306, DateTimeKind.Local).AddTicks(7227)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_roleurl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_systemIndexSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    Index_System_Menu = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    Index_Site_Menu = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    Index_Shortcut = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    Index_FileManage = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    AdminId = table.Column<long>(type: "BIGINT", nullable: false),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 305, DateTimeKind.Local).AddTicks(6279)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_systemIndexSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_testTransaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    Password = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_testTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_workflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Dm:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    WorkFlowContent = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    stepDesign = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    actDesign = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    actionString = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    Introduction = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 307, DateTimeKind.Local).AddTicks(1104)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_workflow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_workflowaction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    actionPathId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    actionName = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    flowId = table.Column<int>(type: "INT", nullable: false),
                    conjunctManFlag = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    directMode = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    orgBossMode = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    stepFromId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    stepToId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    actionFromName = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    actionToName = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    stepToCate = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 306, DateTimeKind.Local).AddTicks(9614)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_workflowaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_core_workflowstep",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    stepPathId = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    stepName = table.Column<string>(type: "NVARCHAR2(8188)", nullable: false),
                    flowId = table.Column<int>(type: "INT", nullable: false),
                    avoidFlag = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    orgMode = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    stepMan = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    stepOrg = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    stepRole = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    stepCate = table.Column<string>(type: "NVARCHAR2(8188)", nullable: true),
                    isStart = table.Column<int>(type: "INT", nullable: false),
                    AddUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    AddUser = table.Column<long>(type: "BIGINT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2024, 3, 12, 21, 30, 36, 307, DateTimeKind.Local).AddTicks(2352)),
                    LastEditUserName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    LastEditUser = table.Column<long>(type: "BIGINT", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    StatusCode = table.Column<int>(type: "INT", nullable: true, defaultValue: 1),
                    Version = table.Column<int>(type: "INT", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_core_workflowstep", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "tbl_core_admin",
                columns: new[] { "Id", "Account", "AddTime", "AddUser", "AddUserName", "AllowMultiLogin", "CurrentLoginIP", "CurrentLoginTime", "FilterIp", "LastEditDate", "LastEditUser", "LastEditUserName", "LockTime", "LoginLogString", "Mutiloginccode", "Password", "RoleId", "RoleName", "SaltValue", "UserAvatar", "UserName", "UserSign", "Version" },
                values: new object[] { 1560206066204151804L, "webmaster", new DateTime(2024, 3, 12, 21, 30, 36, 304, DateTimeKind.Local).AddTicks(3867), 1560206066204151804L, "webmaster", true, "127.0.0.1", new DateTime(2024, 3, 12, 21, 30, 36, 304, DateTimeKind.Local).AddTicks(3867), null, null, 1560206066204151804L, "webmaster", null, null, "7675038.28325281", "4539390B11E07C307C7A2C4224DF3109", 0, "超级管理员", "4ad9879fb285407f", null, "超级管理员", null, 0 });

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
