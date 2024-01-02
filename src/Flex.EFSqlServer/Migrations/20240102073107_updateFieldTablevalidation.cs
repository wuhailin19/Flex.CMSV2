using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldTablevalidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 15, 31, 7, 96, DateTimeKind.Local).AddTicks(8694), new DateTime(2024, 1, 2, 15, 31, 7, 96, DateTimeKind.Local).AddTicks(8694) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 14, 38, 24, 648, DateTimeKind.Local).AddTicks(7024), new DateTime(2024, 1, 2, 14, 38, 24, 648, DateTimeKind.Local).AddTicks(7024) });
        }
    }
}
