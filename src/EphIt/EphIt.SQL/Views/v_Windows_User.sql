CREATE VIEW [dbo].[v_Windows_User]
	AS
SELECT 
	win.User_Id
	,win.UserName
	,win.DisplayName
	,win.Email
	,win.SID AS 'UniqueIdentifier'
	,win.Domain
	,win.FirstName
	,win.LastName
FROM [dbo].User_Windows win
