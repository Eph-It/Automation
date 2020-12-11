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
