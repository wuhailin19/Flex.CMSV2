using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableContentmodelFiledsNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descriptiton",
                table: "tbl_core_contentmodel",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 29, 13, 47, 54, 590, DateTimeKind.Local).AddTicks(8124), new DateTime(2023, 12, 29, 13, 47, 54, 590, DateTimeKind.Local).AddTicks(8124) });

            migrationBuilder.UpdateData(
                table: "tbl_core_menu",
                keyColumn: "Id",
                keyValue: 5,
                column: "LinkUrl",
                value: "columnCategory");

            migrationBuilder.UpdateData(
                table: "tbl_core_menu",
                keyColumn: "Id",
                keyValue: 67,
                column: "LinkUrl",
                value: "ContentModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descriptiton",
                table: "tbl_core_contentmodel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 29, 11, 28, 36, 395, DateTimeKind.Local).AddTicks(5106), new DateTime(2023, 12, 29, 11, 28, 36, 395, DateTimeKind.Local).AddTicks(5106) });

            migrationBuilder.UpdateData(
                table: "tbl_core_menu",
                keyColumn: "Id",
                keyValue: 5,
                column: "LinkUrl",
                value: "columnCategoryContent.aspx?SystemID=1");

            migrationBuilder.UpdateData(
                table: "tbl_core_menu",
                keyColumn: "Id",
                keyValue: 67,
                column: "LinkUrl",
                value: "contentModel.aspx");
        }
    }
}
