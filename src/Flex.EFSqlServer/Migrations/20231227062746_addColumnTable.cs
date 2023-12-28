using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class addColumnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_core_column",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ColumnUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_tbl_core_column", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 27, 14, 27, 46, 460, DateTimeKind.Local).AddTicks(7209), new DateTime(2023, 12, 27, 14, 27, 46, 460, DateTimeKind.Local).AddTicks(7209) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_core_column");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 21, 17, 27, 0, 983, DateTimeKind.Local).AddTicks(5792), new DateTime(2023, 12, 21, 17, 27, 0, 983, DateTimeKind.Local).AddTicks(5792) });
        }
    }
}
