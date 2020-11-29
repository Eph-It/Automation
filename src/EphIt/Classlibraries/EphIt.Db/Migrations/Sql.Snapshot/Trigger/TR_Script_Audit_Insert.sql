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
