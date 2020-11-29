using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EphIt.Db.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authentication",
                columns: table => new
                {
                    AuthenticationId = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.AuthenticationId);
                });

            migrationBuilder.CreateTable(
                name: "JobStatus",
                columns: table => new
                {
                    JobStatusId = table.Column<short>(nullable: false),
                    Status = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStatus", x => x.JobStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RbacAction",
                columns: table => new
                {
                    RbacActionId = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RbacAction", x => x.RbacActionId);
                });

            migrationBuilder.CreateTable(
                name: "RbacObject",
                columns: table => new
                {
                    RbacObjectId = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RbacObject", x => x.RbacObjectId);
                });

            migrationBuilder.CreateTable(
                name: "ScriptLanguage",
                columns: table => new
                {
                    ScriptLanguageId = table.Column<short>(nullable: false),
                    Language = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptLanguage", x => x.ScriptLanguageId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthenticationId = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Authentication_AuthenticationId",
                        column: x => x.AuthenticationId,
                        principalTable: "Authentication",
                        principalColumn: "AuthenticationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Audit_Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RbacActionId = table.Column<short>(nullable: false),
                    RbacObjectId = table.Column<short>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ObjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Audit_Id);
                    table.ForeignKey(
                        name: "FK_Audit_RbacAction_RbacActionId",
                        column: x => x.RbacActionId,
                        principalTable: "RbacAction",
                        principalColumn: "RbacActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Audit_RbacObject_RbacObjectId",
                        column: x => x.RbacObjectId,
                        principalTable: "RbacObject",
                        principalColumn: "RbacObjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Audit_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    IsGlobal = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Role_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Script",
                columns: table => new
                {
                    ScriptId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    ModifiedByUserId = table.Column<int>(nullable: false),
                    PublishedVersion = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Script", x => x.ScriptId);
                    table.ForeignKey(
                        name: "FK_Script_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Script_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserActiveDirectory",
                columns: table => new
                {
                    UserActiveDirectoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 64, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    Domain = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 250, nullable: true),
                    SID = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActiveDirectory", x => x.UserActiveDirectoryId);
                    table.ForeignKey(
                        name: "FK_UserActiveDirectory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAzureActiveDirectory",
                columns: table => new
                {
                    UserAzureActiveDirectoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    TenantId = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    ObjectId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAzureActiveDirectory", x => x.UserAzureActiveDirectoryId);
                    table.ForeignKey(
                        name: "FK_UserAzureActiveDirectory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleMembershipUser",
                columns: table => new
                {
                    RoleMembershipUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMembershipUser", x => x.RoleMembershipUserId);
                    table.ForeignKey(
                        name: "FK_RoleMembershipUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMembershipUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleObjectAction",
                columns: table => new
                {
                    RoleObjectActionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RbacActionId = table.Column<short>(nullable: false),
                    RbacObjectId = table.Column<short>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleObjectAction", x => x.RoleObjectActionId);
                    table.ForeignKey(
                        name: "FK_RoleObjectAction_RbacAction_RbacActionId",
                        column: x => x.RbacActionId,
                        principalTable: "RbacAction",
                        principalColumn: "RbacActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleObjectAction_RbacObject_RbacObjectId",
                        column: x => x.RbacObjectId,
                        principalTable: "RbacObject",
                        principalColumn: "RbacObjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleObjectAction_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleObjectScopeScript",
                columns: table => new
                {
                    RoleObjectScopeScriptId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScriptId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleObjectScopeScript", x => x.RoleObjectScopeScriptId);
                    table.ForeignKey(
                        name: "FK_RoleObjectScopeScript_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleObjectScopeScript_Script_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Script",
                        principalColumn: "ScriptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScriptVersion",
                columns: table => new
                {
                    ScriptVersionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Body = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    ScriptLanguageId = table.Column<short>(nullable: true),
                    ScriptId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptVersion", x => x.ScriptVersionId);
                    table.ForeignKey(
                        name: "FK_ScriptVersion_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScriptVersion_Script_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Script",
                        principalColumn: "ScriptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScriptVersion_ScriptLanguage_ScriptLanguageId",
                        column: x => x.ScriptLanguageId,
                        principalTable: "ScriptLanguage",
                        principalColumn: "ScriptLanguageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    JobUid = table.Column<Guid>(nullable: false),
                    ScriptVersionId = table.Column<int>(nullable: false),
                    JobStatusId = table.Column<short>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedByScheduleId = table.Column<int>(nullable: true),
                    CreatedByAutomationId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Start = table.Column<DateTime>(nullable: true),
                    Finish = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobUid);
                    table.ForeignKey(
                        name: "FK_Job_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Job_JobStatus_JobStatusId",
                        column: x => x.JobStatusId,
                        principalTable: "JobStatus",
                        principalColumn: "JobStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Job_ScriptVersion_ScriptVersionId",
                        column: x => x.ScriptVersionId,
                        principalTable: "ScriptVersion",
                        principalColumn: "ScriptVersionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobLog",
                columns: table => new
                {
                    JobLogId = table.Column<Guid>(nullable: false),
                    JobUid = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    LevelId = table.Column<short>(nullable: false),
                    LogTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLog", x => x.JobLogId);
                    table.ForeignKey(
                        name: "FK_JobLog_Job_JobUid",
                        column: x => x.JobUid,
                        principalTable: "Job",
                        principalColumn: "JobUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobParameters",
                columns: table => new
                {
                    JobUid = table.Column<Guid>(nullable: false),
                    Parameters = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobParameters", x => x.JobUid);
                    table.ForeignKey(
                        name: "FK_JobParameters_Job_JobUid",
                        column: x => x.JobUid,
                        principalTable: "Job",
                        principalColumn: "JobUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobQueue",
                columns: table => new
                {
                    JobUid = table.Column<Guid>(nullable: false),
                    ScriptVersionId = table.Column<int>(nullable: false),
                    ScriptLanguage = table.Column<short>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQueue", x => x.JobUid);
                    table.ForeignKey(
                        name: "FK_JobQueue_Job_JobUid",
                        column: x => x.JobUid,
                        principalTable: "Job",
                        principalColumn: "JobUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authentication",
                columns: new[] { "AuthenticationId", "Name" },
                values: new object[,]
                {
                    { (short)1, "ActiveDirectory" },
                    { (short)2, "EphItInternal" },
                    { (short)3, "AzureActiveDirectory" }
                });

            migrationBuilder.InsertData(
                table: "JobStatus",
                columns: new[] { "JobStatusId", "Status" },
                values: new object[,]
                {
                    { (short)1, "New" },
                    { (short)2, "Queued" },
                    { (short)3, "InProgress" },
                    { (short)10, "Complete" },
                    { (short)11, "Error" },
                    { (short)12, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "RbacAction",
                columns: new[] { "RbacActionId", "Name" },
                values: new object[,]
                {
                    { (short)5, "Execute" },
                    { (short)4, "Modify" },
                    { (short)2, "Read" },
                    { (short)1, "Create" },
                    { (short)3, "Delete" }
                });

            migrationBuilder.InsertData(
                table: "RbacObject",
                columns: new[] { "RbacObjectId", "Name" },
                values: new object[,]
                {
                    { (short)1, "Scripts" },
                    { (short)2, "Roles" },
                    { (short)3, "Variables" },
                    { (short)4, "Modules" },
                    { (short)5, "Jobs" }
                });

            migrationBuilder.InsertData(
                table: "ScriptLanguage",
                columns: new[] { "ScriptLanguageId", "Language" },
                values: new object[,]
                {
                    { (short)1, "PowerShell" },
                    { (short)2, "PowerShellCore" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "AuthenticationId" },
                values: new object[] { -1, (short)2 });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_RbacActionId",
                table: "Audit",
                column: "RbacActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_RbacObjectId",
                table: "Audit",
                column: "RbacObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_UserId",
                table: "Audit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_CreatedByUserId",
                table: "Job",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_JobStatusId",
                table: "Job",
                column: "JobStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_ScriptVersionId",
                table: "Job",
                column: "ScriptVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLog_JobUid",
                table: "JobLog",
                column: "JobUid");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedByUserId",
                table: "Role",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ModifiedByUserId",
                table: "Role",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMembershipUser_RoleId",
                table: "RoleMembershipUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMembershipUser_UserId",
                table: "RoleMembershipUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleObjectAction_RbacActionId",
                table: "RoleObjectAction",
                column: "RbacActionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleObjectAction_RbacObjectId",
                table: "RoleObjectAction",
                column: "RbacObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleObjectAction_RoleId",
                table: "RoleObjectAction",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleObjectScopeScript_RoleId",
                table: "RoleObjectScopeScript",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleObjectScopeScript_ScriptId",
                table: "RoleObjectScopeScript",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Script_CreatedByUserId",
                table: "Script",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Script_ModifiedByUserId",
                table: "Script",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptVersion_CreatedByUserId",
                table: "ScriptVersion",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptVersion_ScriptId",
                table: "ScriptVersion",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptVersion_ScriptLanguageId",
                table: "ScriptVersion",
                column: "ScriptLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AuthenticationId",
                table: "User",
                column: "AuthenticationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActiveDirectory_UserId",
                table: "UserActiveDirectory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAzureActiveDirectory_UserId",
                table: "UserAzureActiveDirectory",
                column: "UserId");



            migrationBuilder.Sql(@"
            
            CREATE OR ALTER TRIGGER dbo.TR_Script_Audit_Insert
            ON Script
            AFTER INSERT
            AS BEGIN
            
            INSERT INTO [AUDIT] ( RbacActionId, RbacObjectId, Created, UserId, ObjectId )
            	SELECT
            		1 AS 'RbacActionId'
            		,1 AS 'RbacObjectId'
            		,GETUTCDATE() AS Created
            		,INSERTED.CreatedByUserId AS UserId
            		,INSERTED.ScriptId AS ObjectId
            	FROM INSERTED
            END
            
            
            ");
            migrationBuilder.Sql(@"
            
            CREATE OR ALTER TRIGGER dbo.TR_Script_Audit_Update
            ON Script
            AFTER UPDATE
            AS BEGIN
            
            INSERT INTO [AUDIT] ( RbacActionId, RbacObjectId, Created, UserId, ObjectId )
            	SELECT
            		CASE
            			WHEN INSERTED.IsDeleted = 1 THEN 3
            			ELSE 4
            		END AS 'RbacActionId'
            		,1 AS 'RbacObjectId'
            		,GETUTCDATE() AS Created
            		,INSERTED.ModifiedByUserId AS UserId
            		,INSERTED.ScriptId AS ObjectId
            	FROM INSERTED
            END
            
            
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "JobLog");

            migrationBuilder.DropTable(
                name: "JobParameters");

            migrationBuilder.DropTable(
                name: "JobQueue");

            migrationBuilder.DropTable(
                name: "RoleMembershipUser");

            migrationBuilder.DropTable(
                name: "RoleObjectAction");

            migrationBuilder.DropTable(
                name: "RoleObjectScopeScript");

            migrationBuilder.DropTable(
                name: "UserActiveDirectory");

            migrationBuilder.DropTable(
                name: "UserAzureActiveDirectory");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "RbacAction");

            migrationBuilder.DropTable(
                name: "RbacObject");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "JobStatus");

            migrationBuilder.DropTable(
                name: "ScriptVersion");

            migrationBuilder.DropTable(
                name: "Script");

            migrationBuilder.DropTable(
                name: "ScriptLanguage");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Authentication");
        }
    }
}



