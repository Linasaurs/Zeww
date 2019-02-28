using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zeww.Migrations
{
    public partial class @base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "Workspaces",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DailyDoNotDisturbFrom",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DailyDoNotDisturbTo",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatorID",
                table: "Chats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Chats",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Chats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URL",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "DailyDoNotDisturbFrom",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DailyDoNotDisturbTo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatorID",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Chats");
        }
    }
}
