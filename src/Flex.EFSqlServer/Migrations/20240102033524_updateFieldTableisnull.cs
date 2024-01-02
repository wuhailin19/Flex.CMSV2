using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldTableisnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSearch",
                table: "tbl_core_field",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsApiField",
                table: "tbl_core_field",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 11, 35, 24, 211, DateTimeKind.Local).AddTicks(6715), new DateTime(2024, 1, 2, 11, 35, 24, 211, DateTimeKind.Local).AddTicks(6715) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSearch",
                table: "tbl_core_field",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsApiField",
                table: "tbl_core_field",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 11, 21, 10, 530, DateTimeKind.Local).AddTicks(6220), new DateTime(2024, 1, 2, 11, 21, 10, 530, DateTimeKind.Local).AddTicks(6220) });
        }
    }
}
