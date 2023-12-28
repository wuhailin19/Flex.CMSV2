using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class addColumnTableFileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColumnImage",
                table: "tbl_core_column",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExtensionModelId",
                table: "tbl_core_column",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "tbl_core_column",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoKeyWord",
                table: "tbl_core_column",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "tbl_core_column",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 28, 10, 59, 25, 620, DateTimeKind.Local).AddTicks(2887), new DateTime(2023, 12, 28, 10, 59, 25, 620, DateTimeKind.Local).AddTicks(2887) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnImage",
                table: "tbl_core_column");

            migrationBuilder.DropColumn(
                name: "ExtensionModelId",
                table: "tbl_core_column");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "tbl_core_column");

            migrationBuilder.DropColumn(
                name: "SeoKeyWord",
                table: "tbl_core_column");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "tbl_core_column");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 27, 14, 58, 24, 985, DateTimeKind.Local).AddTicks(226), new DateTime(2023, 12, 27, 14, 58, 24, 985, DateTimeKind.Local).AddTicks(226) });
        }
    }
}
