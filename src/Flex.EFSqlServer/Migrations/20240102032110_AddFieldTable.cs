using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flex.EFSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descriptiton",
                table: "tbl_core_contentmodel",
                newName: "Description");

            migrationBuilder.CreateTable(
                name: "tbl_core_field",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FieldDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FieldType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Validation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FieldAttritude = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApiName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApiField = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsSearch = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_tbl_core_field", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2024, 1, 2, 11, 21, 10, 530, DateTimeKind.Local).AddTicks(6220), new DateTime(2024, 1, 2, 11, 21, 10, 530, DateTimeKind.Local).AddTicks(6220) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_core_field");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "tbl_core_contentmodel",
                newName: "Descriptiton");

            migrationBuilder.UpdateData(
                table: "tbl_core_admin",
                keyColumn: "Id",
                keyValue: 1560206066204151804L,
                columns: new[] { "AddTime", "CurrentLoginTime" },
                values: new object[] { new DateTime(2023, 12, 29, 13, 47, 54, 590, DateTimeKind.Local).AddTicks(8124), new DateTime(2023, 12, 29, 13, 47, 54, 590, DateTimeKind.Local).AddTicks(8124) });
        }
    }
}
