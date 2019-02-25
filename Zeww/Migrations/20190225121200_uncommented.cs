using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class uncommented : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Files",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Files",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ChatId",
                table: "Files",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserId",
                table: "Files",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Chats_ChatId",
                table: "Files",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_UserId",
                table: "Files",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Chats_ChatId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_UserId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ChatId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_UserId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Files");
        }
    }
}
