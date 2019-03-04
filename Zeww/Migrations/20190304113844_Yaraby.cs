using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class Yaraby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShowHideMails",
                table: "Workspaces",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowHideMails",
                table: "Workspaces");
        }
    }
}
