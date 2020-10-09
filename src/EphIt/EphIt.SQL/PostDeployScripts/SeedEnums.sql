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

INSERT INTO dbo.Authentication (Authentication_Id, Name)
SELECT 1, N'Windows'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Authentication WHERE Authentication_Id = 1 )

INSERT INTO dbo.Authentication (Authentication_Id, Name)
SELECT 2, N'EphItInternal'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Authentication WHERE Authentication_Id = 2 )

INSERT INTO dbo.Script_Language ( Script_Language_Id, Language, Version )
SELECT 1, 'PowerShell', '5.1'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Script_Language WHERE Script_Language_Id = 1 )

INSERT INTO dbo.Job_Status ( Job_Status_Id, Status )
SELECT 1, 'New'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Job_Status WHERE Job_Status_Id = 1 )

INSERT INTO dbo.Job_Status ( Job_Status_Id, Status )
SELECT 2, 'Queued'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Job_Status WHERE Job_Status_Id = 2 )

INSERT INTO dbo.Job_Status ( Job_Status_Id, Status )
SELECT 3, 'In Progress'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Job_Status WHERE Job_Status_Id = 3 )

INSERT INTO dbo.Job_Status ( Job_Status_Id, Status )
SELECT 10, 'Complete'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Job_Status WHERE Job_Status_Id = 10 )

INSERT INTO dbo.Job_Status ( Job_Status_Id, Status )
SELECT 11, 'Error'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Job_Status WHERE Job_Status_Id = 11 )

INSERT INTO dbo.Job_Status ( Job_Status_Id, Status )
SELECT 12, 'Cancelled'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Job_Status WHERE Job_Status_Id = 12 )

INSERT INTO dbo.RBAC_Action (RBAC_Action_Id, Name)
SELECT 1, 'Create'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Action WHERE RBAC_Action_Id = 1 )

INSERT INTO dbo.RBAC_Action (RBAC_Action_Id, Name)
SELECT 2, 'Read'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Action WHERE RBAC_Action_Id = 2 )

INSERT INTO dbo.RBAC_Action (RBAC_Action_Id, Name)
SELECT 3, 'Delete'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Action WHERE RBAC_Action_Id = 3 )

INSERT INTO dbo.RBAC_Action (RBAC_Action_Id, Name)
SELECT 4, 'Modify'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Action WHERE RBAC_Action_Id = 4 )

INSERT INTO dbo.RBAC_Action (RBAC_Action_Id, Name)
SELECT 5, 'Execute'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Action WHERE RBAC_Action_Id = 5 )

INSERT INTO dbo.RBAC_Object (RBAC_Object_Id, Name)
SELECT 1, 'Scripts'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Object WHERE RBAC_Object_Id = 1 )

INSERT INTO dbo.RBAC_Object (RBAC_Object_Id, Name)
SELECT 2, 'Roles'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Object WHERE RBAC_Object_Id = 2 )

INSERT INTO dbo.RBAC_Object (RBAC_Object_Id, Name)
SELECT 3, 'Variables'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Object WHERE RBAC_Object_Id = 3 )

INSERT INTO dbo.RBAC_Object (RBAC_Object_Id, Name)
SELECT 4, 'Modules'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Object WHERE RBAC_Object_Id = 4 )

INSERT INTO dbo.RBAC_Object (RBAC_Object_Id, Name)
SELECT 5, 'Jobs'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.RBAC_Object WHERE RBAC_Object_Id = 5 )

SET IDENTITY_INSERT [User] ON
INSERT INTO dbo.[User] ([User_Id], Authentication_Id)
SELECT -1, 2
WHERE NOT EXISTS ( SELECT 1 FROM dbo.[User] WHERE [User_Id] = -1 )
SET IDENTITY_INSERT [User] OFF

SET IDENTITY_INSERT [Role] ON
INSERT INTO dbo.Role ( Role_Id, Created_By_User_Id, Description, IsGlobal, Modified_By_User_Id, Name )
SELECT 1, -1, N'Full administrator of all objects', 1, -1, 'Administrators'
WHERE NOT EXISTS ( SELECT 1 FROM dbo.Role WHERE Role_Id = 1 )
SET IDENTITY_INSERT [Role] OFF
