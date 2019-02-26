using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class ModifiedWorspaceAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DateOfCreation",
                table: "Workspaces",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfCreation",
                table: "Workspaces",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
