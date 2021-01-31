CREATE OR ALTER VIEW v_RBACScriptVersionToObjectId
AS
SELECT 
	rsr.ScriptId
	,rsr.ScriptVersionId
	,mu.RoleId
	,mu.UserId as UserGroupId
	,1 AS 'IsUser'
	,ruo.ObjectId
FROM v_RBACScriptVersionRole rsr
JOIN RoleMembershipUser mu ON mu.RoleId = rsr.RoleId
FULL JOIN v_RBACUserObjectId ruo ON ruo.UserId = mu.UserId
