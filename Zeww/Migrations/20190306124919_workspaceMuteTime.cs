using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class workspaceMuteTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToWhichNotificationsAreMuted",
                table: "UserWorkspaces",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToWhichNotificationsAreMuted",
                table: "UserWorkspaces");
        }
    }
}
