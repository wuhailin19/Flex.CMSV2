using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class updateAdminCurrentiptime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastLoginTime",
                table: "tbl_core_admin",
                newName: "CurrentLoginTime");

            migrationBuilder.RenameColumn(
                name: "LastLoginIP",
                table: "tbl_core_admin",
                newName: "CurrentLoginIP");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 21, 17, 23, 59, 35, DateTimeKind.Local).AddTicks(6863), new DateTime(2023, 12, 21, 17, 23, 59, 35, DateTimeKind.Local).AddTicks(6863) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentLoginTime",
                table: "tbl_core_admin",
                newName: "LastLoginTime");

            migrationBuilder.RenameColumn(
                name: "CurrentLoginIP",
                table: "tbl_core_admin",
                newName: "LastLoginIP");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 12, 21, 17, 18, 19, 399, DateTimeKind.Local).AddTicks(8755), new DateTime(2023, 12, 21, 17, 18, 19, 399, DateTimeKind.Local).AddTicks(8755) });
        }
    }
}
