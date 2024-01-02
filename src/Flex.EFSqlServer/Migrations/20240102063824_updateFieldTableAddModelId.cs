using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldTableAddModelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "tbl_core_field",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 14, 38, 24, 648, DateTimeKind.Local).AddTicks(7024), new DateTime(2024, 1, 2, 14, 38, 24, 648, DateTimeKind.Local).AddTicks(7024) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "tbl_core_field");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 14, 27, 29, 959, DateTimeKind.Local).AddTicks(575), new DateTime(2024, 1, 2, 14, 27, 29, 959, DateTimeKind.Local).AddTicks(575) });
        }
    }
}
