using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class AddedUserWorkspaceRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkspace_Users_UserId",
                table: "UserWorkspace");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkspace_Workspaces_WorkspaceId",
                table: "UserWorkspace");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserWorkspace_UserId_WorkspaceId",
                table: "UserWorkspace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWorkspace",
                table: "UserWorkspace");

            migrationBuilder.RenameTable(
                name: "UserWorkspace",
                newName: "UserWorkspaces");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserWorkspaces_UserId_WorkspaceId",
                table: "UserWorkspaces",
                columns: new[] { "UserId", "WorkspaceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWorkspaces",
                table: "UserWorkspaces",
                columns: new[] { "WorkspaceId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkspaces_Users_UserId",
                table: "UserWorkspaces",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkspaces_Workspaces_WorkspaceId",
                table: "UserWorkspaces",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkspaces_Users_UserId",
                table: "UserWorkspaces");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkspaces_Workspaces_WorkspaceId",
                table: "UserWorkspaces");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserWorkspaces_UserId_WorkspaceId",
                table: "UserWorkspaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWorkspaces",
                table: "UserWorkspaces");

            migrationBuilder.RenameTable(
                name: "UserWorkspaces",
                newName: "UserWorkspace");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserWorkspace_UserId_WorkspaceId",
                table: "UserWorkspace",
                columns: new[] { "UserId", "WorkspaceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWorkspace",
                table: "UserWorkspace",
                columns: new[] { "WorkspaceId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkspace_Users_UserId",
                table: "UserWorkspace",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkspace_Workspaces_WorkspaceId",
                table: "UserWorkspace",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
