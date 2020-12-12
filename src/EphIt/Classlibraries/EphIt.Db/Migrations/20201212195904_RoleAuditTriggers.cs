using Microsoft.EntityFrameworkCore.Migrations;

namespace EphIt.Db.Migrations
{
    public partial class RoleAuditTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.Sql(@"
            
            CREATE OR ALTER TRIGGER dbo.TR_Role_Audit_Insert
            ON [Role]
            AFTER INSERT
            AS BEGIN
            
            INSERT INTO [AUDIT] ( RbacActionId, RbacObjectId, Created, UserId, ObjectId )
            	SELECT
            		1 AS 'RbacActionId'
            		,2 AS 'RbacObjectId'
            		,GETUTCDATE() AS Created
            		,INSERTED.CreatedByUserId AS UserId
            		,INSERTED.RoleId AS ObjectId
            	FROM INSERTED
            END
            
            
            ");
            migrationBuilder.Sql(@"
            
            CREATE OR ALTER TRIGGER dbo.TR_Role_Audit_Update
            ON [Role]
            AFTER UPDATE
            AS BEGIN
            
            INSERT INTO [AUDIT] ( RbacActionId, RbacObjectId, Created, UserId, ObjectId )
            	SELECT
            		CASE
            			WHEN INSERTED.IsDeleted = 1 THEN 3
            			ELSE 4
            		END AS 'RbacActionId'
            		,2 AS 'RbacObjectId'
            		,GETUTCDATE() AS Created
            		,INSERTED.ModifiedByUserId AS UserId
            		,INSERTED.RoleId AS ObjectId
            	FROM INSERTED
            END
            
            
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
            
            DROP Trigger IF EXISTS TR_Role_Audit_Insert
            
            ");
            migrationBuilder.Sql(@"
            
            DROP Trigger IF EXISTS TR_Role_Audit_Update
            
            ");

        }
    }
}

