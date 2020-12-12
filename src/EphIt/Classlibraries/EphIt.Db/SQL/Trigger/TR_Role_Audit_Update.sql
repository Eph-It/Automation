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
