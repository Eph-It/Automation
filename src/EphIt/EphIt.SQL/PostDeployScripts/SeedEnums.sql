/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


SET IDENTITY_INSERT [Role] ON
INSERT INTO dbo.Role ( Role_Id, Created_By_User_Id, Description, IsGlobal, Modified_By_User_Id, Name )
SELECT 1, -1, N'Full administrator of all objects', 1, -1, 'Administrators'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Role WHERE Role_Id = 1 )
SET IDENTITY_INSERT [Role] OFF

DECLARE @ActionId int

DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT [RBAC_Action_Id]
  FROM [dbo].[RBAC_Action]

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @ActionId
WHILE @@FETCH_STATUS = 0
BEGIN 
	INSERT INTO [dbo].[Role_Object_Action] (Role_Id, RBAC_Action_Id, RBAC_Object_Id)
	SELECT 
		1 AS Role_Id
		,@ActionId as 'RBAC_Action_Id'
		,ro.RBAC_Object_Id
	FROM [dbo].[RBAC_Object] ro
	LEFT JOIN [dbo].[Role_Object_Action] ra ON ra.RBAC_Action_Id = @ActionId AND ro.RBAC_Object_Id = ra.RBAC_Object_Id AND ra.Role_Id = 1
	WHERE ra.Role_Id IS NULL
    FETCH NEXT FROM MY_CURSOR INTO @ActionId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR