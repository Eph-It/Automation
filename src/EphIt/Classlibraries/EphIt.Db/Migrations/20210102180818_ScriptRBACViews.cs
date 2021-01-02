using Microsoft.EntityFrameworkCore.Migrations;

namespace EphIt.Db.Migrations
{
    public partial class ScriptRBACViews : Migration
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
            
            CREATE OR ALTER VIEW v_RBACScriptRole
            AS
            
            WITH CTE AS (
            	SELECT 
            		[Role].RoleId
            		,[Script].ScriptId
            	FROM [Script]
            	CROSS JOIN [Role]
            	WHERE [ROLE].IsGlobal = 1
            
            	UNION
            
            	SELECT 
            		rss.RoleId
            		,rss.ScriptId
            	FROM RoleObjectScopeScript rss
            )
            SELECT 
            	ROW_NUMBER() OVER (Order by RoleId) as 'RowId'
            	,RoleId
            	,ScriptId
            
            FROM CTE
            
            ");

            migrationBuilder.Sql(@"
            
            CREATE OR ALTER VIEW v_RBACUserObjectId
            AS
            
            WITH CTE AS (
            	SELECT u.UserId
            		   ,uad.SID as 'ObjectId'
            	FROM [User] u
            	JOIN [UserActiveDirectory] uad ON uad.UserId = u.UserId
            	UNION
            	SELECT u.UserId
            		   ,uaad.ObjectId as 'ObjectId'
            	FROM [User] u
            	JOIN UserAzureActiveDirectory uaad ON uaad.UserId = u.UserId
            )
            
            SELECT
            	ROW_NUMBER() OVER (Order by UserId) as 'RowId'
            	,UserId
            	,ObjectId
            FROM CTE
            
            
            ");

            migrationBuilder.Sql(@"
            
            CREATE OR ALTER VIEW v_RBACScriptToObjectId
            AS
            SELECT 
            	rsr.ScriptId
            	,mu.RoleId
            	,mu.UserId as UserGroupId
            	,1 AS 'IsUser'
            	,ruo.ObjectId
            FROM v_RBACScriptRole rsr
            JOIN RoleMembershipUser mu ON mu.RoleId = rsr.RoleId
            FULL JOIN v_RBACUserObjectId ruo ON ruo.UserId = mu.UserId
            
            
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
            		,INSERTED.RoleIdId AS ObjectId
            	FROM INSERTED
            END
            
            
            ");
            migrationBuilder.Sql(@"
            
            DROP View IF EXISTS v_RBACScriptToObjectId
            
            ");
            migrationBuilder.Sql(@"
            
            DROP View IF EXISTS v_RBACScriptRole
            
            ");
            
            migrationBuilder.Sql(@"
            
            DROP View IF EXISTS v_RBACUserObjectId
            
            ");

        }
    }
}

