CREATE VIEW [dbo].[v_User]
	AS 

SELECT
	winUser.User_Id
	,'Windows' AS 'AuthenticationType'
	,CAST(1 AS smallint) AS 'AuthenticationId'
	,winUser.DisplayName
	,winUser.UserName
	,winUser.Email
	,winUser.UniqueIdentifier
FROM v_Windows_User winUser