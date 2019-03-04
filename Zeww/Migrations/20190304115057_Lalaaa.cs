using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class Lalaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowHideMails",
                table: "Workspaces");

            migrationBuilder.AddColumn<int>(
                name: "ShowHideEmails",
                table: "Workspaces",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowHideEmails",
                table: "Workspaces");

            migrationBuilder.AddColumn<int>(
                name: "ShowHideMails",
                table: "Workspaces",
                nullable: false,
                defaultValue: 0);
        }
    }
}
