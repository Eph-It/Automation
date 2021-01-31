CREATE OR ALTER VIEW v_RBACScriptVersionRole
AS
WITH CTE AS (
	SELECT
		sr.RoleId
		,sr.ScriptId
		,sv.ScriptVersionId
	FROM v_RBACScriptRole sr
	RIGHT JOIN ScriptVersion sv ON sv.ScriptId = sr.ScriptId
)
SELECT 
	ROW_NUMBER() OVER (Order by RoleId) as 'RowId'
	,RoleId
	,ScriptId
	,ScriptVersionId
FROM CTE