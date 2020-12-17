using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EphIt.Db.Migrations
{
    public partial class variableModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Variable");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Variable",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Variable",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Variable",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "Variable",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Variable");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Variable");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Variable");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Variable");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Variable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
