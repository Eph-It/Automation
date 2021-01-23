CREATE OR ALTER VIEW v_RBACJobRole
AS
WITH CTE AS (
	SELECT
		sr.RoleId
		,sr.ScriptId
		,j.ScriptVersionId
		,j.JobUid
	FROM v_RBACScriptVersionRole sr
	RIGHT JOIN Job j ON j.ScriptVersionId = sr.ScriptVersionId
)
SELECT 
	ROW_NUMBER() OVER (Order by RoleId) as 'RowId'
	,RoleId
	,ScriptId
	,ScriptVersionId
	,JobUid
FROM CTE