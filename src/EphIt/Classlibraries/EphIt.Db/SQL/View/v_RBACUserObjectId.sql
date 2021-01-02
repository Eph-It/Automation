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
