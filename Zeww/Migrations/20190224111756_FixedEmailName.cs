using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class FixedEmailName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");
        }
    }
}
