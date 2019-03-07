using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class wsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkspaceImage",
                table: "Workspaces");

            migrationBuilder.AddColumn<string>(
                name: "WorkspaceImageId",
                table: "Workspaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkspaceImageName",
                table: "Workspaces",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkspaceImageId",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "WorkspaceImageName",
                table: "Workspaces");

            migrationBuilder.AddColumn<byte[]>(
                name: "WorkspaceImage",
                table: "Workspaces",
                nullable: true);
        }
    }
}
