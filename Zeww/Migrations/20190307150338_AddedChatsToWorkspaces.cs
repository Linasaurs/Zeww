using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class AddedChatsToWorkspaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Chats_WorkspaceId",
                table: "Chats",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Workspaces_WorkspaceId",
                table: "Chats",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Workspaces_WorkspaceId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_WorkspaceId",
                table: "Chats");
        }
    }
}
