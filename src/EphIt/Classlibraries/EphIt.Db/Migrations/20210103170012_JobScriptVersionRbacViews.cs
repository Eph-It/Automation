using Microsoft.EntityFrameworkCore.Migrations;

namespace EphIt.Db.Migrations
{
    public partial class JobScriptVersionRbacViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
            
            CREATE OR ALTER VIEW v_RBACScriptVersionRole
            AS
            WITH CTE AS (
            	SELECT
            		sr.RoleId
            		,sr.ScriptId
            		,sv.ScriptVersionId
            	FROM v_RBACScriptRole sr
            	RIGHT JOIN ScriptVersion sv ON sv.ScriptId = sr.ScriptId
            )
            SELECT 
            	ROW_NUMBER() OVER (Order by RoleId) as 'RowId'
            	,RoleId
            	,ScriptId
            	,ScriptVersionId
            FROM CTE
            
            ");
            migrationBuilder.Sql(@"
            
            CREATE OR ALTER VIEW v_RBACScriptVersionToObjectId
            AS
            SELECT 
            	rsr.ScriptId
            	,rsr.ScriptVersionId
            	,mu.RoleId
            	,mu.UserId as UserGroupId
            	,1 AS 'IsUser'
            	,ruo.ObjectId
            FROM v_RBACScriptVersionRole rsr
            JOIN RoleMembershipUser mu ON mu.RoleId = rsr.RoleId
            FULL JOIN v_RBACUserObjectId ruo ON ruo.UserId = mu.UserId
            
            
            ");

            migrationBuilder.Sql(@"
            
            CREATE OR ALTER VIEW v_RBACJobRole
            AS
            WITH CTE AS (
            	SELECT
            		sr.RoleId
            		,sr.ScriptId
            		,j.ScriptVersionId
            		,j.JobUid
            	FROM v_RBACScriptVersionRole sr
            	RIGHT JOIN Job j ON j.ScriptVersionId = sr.ScriptVersionId
            )
            SELECT 
            	ROW_NUMBER() OVER (Order by RoleId) as 'RowId'
            	,RoleId
            	,ScriptId
            	,ScriptVersionId
            	,JobUid
            FROM CTE
            
            ");
            migrationBuilder.Sql(@"
            
            CREATE OR ALTER VIEW v_RBACJobToObjectId
            AS
            SELECT 
            	jr.ScriptId
            	,jr.ScriptVersionId
            	,jr.JobUid
            	,mu.RoleId
            	,mu.UserId as UserGroupId
            	,1 AS 'IsUser'
            	,ruo.ObjectId
            FROM v_RBACJobRole jr
            JOIN RoleMembershipUser mu ON mu.RoleId = jr.RoleId
            FULL JOIN v_RBACUserObjectId ruo ON ruo.UserId = mu.UserId
            
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
            
            DROP View IF EXISTS v_RBACJobToObjectId
            
            ");

            migrationBuilder.Sql(@"
            
            DROP View IF EXISTS v_RBACJobRole
            
            ");
            
            migrationBuilder.Sql(@"
            
            DROP View IF EXISTS v_RBACScriptVersionToObjectId
            
            ");

            migrationBuilder.Sql(@"
            
            DROP View IF EXISTS v_RBACScriptVersionRole
            
            ");
            

        }
    }
}

