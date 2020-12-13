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
