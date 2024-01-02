using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldTableorderiddefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "tbl_core_field",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 11, 48, 18, 241, DateTimeKind.Local).AddTicks(2348), new DateTime(2024, 1, 2, 11, 48, 18, 241, DateTimeKind.Local).AddTicks(2348) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "tbl_core_field",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 11, 35, 24, 211, DateTimeKind.Local).AddTicks(6715), new DateTime(2024, 1, 2, 11, 35, 24, 211, DateTimeKind.Local).AddTicks(6715) });
        }
    }
}
