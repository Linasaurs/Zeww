using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class mi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "WorkspaceImage",
                table: "Workspaces",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkspaceImage",
                table: "Workspaces");
        }
    }
}
