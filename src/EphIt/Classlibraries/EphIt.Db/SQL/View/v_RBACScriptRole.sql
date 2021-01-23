CREATE OR ALTER VIEW v_RBACScriptRole
AS

WITH CTE AS (
	SELECT 
		[Role].RoleId
		,[Script].ScriptId
	FROM [Script]
	CROSS JOIN [Role]
	WHERE [ROLE].IsGlobal = 1

	UNION

	SELECT 
		rss.RoleId
		,rss.ScriptId
	FROM RoleObjectScopeScript rss
)
SELECT 
	ROW_NUMBER() OVER (Order by RoleId) as 'RowId'
	,RoleId
	,ScriptId

FROM CTE