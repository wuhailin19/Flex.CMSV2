using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class addColumnTableIsShowFiled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShow",
                table: "tbl_core_column",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 27, 14, 58, 24, 985, DateTimeKind.Local).AddTicks(226), new DateTime(2023, 12, 27, 14, 58, 24, 985, DateTimeKind.Local).AddTicks(226) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShow",
                table: "tbl_core_column");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 27, 14, 27, 46, 460, DateTimeKind.Local).AddTicks(7209), new DateTime(2023, 12, 27, 14, 27, 46, 460, DateTimeKind.Local).AddTicks(7209) });
        }
    }
}
