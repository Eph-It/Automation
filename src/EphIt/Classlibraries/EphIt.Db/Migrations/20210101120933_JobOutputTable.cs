using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EphIt.Db.Migrations
{
    public partial class JobOutputTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobOutput",
                columns: table => new
                {
                    JobOutputId = table.Column<Guid>(nullable: false),
                    JobUid = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    JsonValue = table.Column<string>(nullable: true),
                    ByteArrayValue = table.Column<byte[]>(nullable: true),
                    OutputTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOutput", x => x.JobOutputId);
                    table.ForeignKey(
                        name: "FK_JobOutput_Job_JobUid",
                        column: x => x.JobUid,
                        principalTable: "Job",
                        principalColumn: "JobUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOutput_JobUid",
                table: "JobOutput",
                column: "JobUid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOutput");
        }
    }
}
