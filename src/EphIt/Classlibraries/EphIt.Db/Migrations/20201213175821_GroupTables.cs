using Microsoft.EntityFrameworkCore.Migrations;

namespace EphIt.Db.Migrations
{
    public partial class GroupTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthenticationId = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Group_Authentication_AuthenticationId",
                        column: x => x.AuthenticationId,
                        principalTable: "Authentication",
                        principalColumn: "AuthenticationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupActiveDirectory",
                columns: table => new
                {
                    GroupActiveDirectoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    DistinguisedName = table.Column<string>(nullable: false),
                    Domain = table.Column<string>(maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 250, nullable: true),
                    SID = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupActiveDirectory", x => x.GroupActiveDirectoryId);
                    table.ForeignKey(
                        name: "FK_GroupActiveDirectory_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupAzureActiveDirectory",
                columns: table => new
                {
                    GroupAzureActiveDirectoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    GroupName = table.Column<string>(maxLength: 256, nullable: false),
                    TenantId = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    ObjectId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAzureActiveDirectory", x => x.GroupAzureActiveDirectoryId);
                    table.ForeignKey(
                        name: "FK_GroupAzureActiveDirectory_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Group",
                columns: new[] { "GroupId", "AuthenticationId" },
                values: new object[] { -1, (short)2 });

            migrationBuilder.CreateIndex(
                name: "IX_Group_AuthenticationId",
                table: "Group",
                column: "AuthenticationId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupActiveDirectory_GroupId",
                table: "GroupActiveDirectory",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAzureActiveDirectory_GroupId",
                table: "GroupAzureActiveDirectory",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupActiveDirectory");

            migrationBuilder.DropTable(
                name: "GroupAzureActiveDirectory");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
