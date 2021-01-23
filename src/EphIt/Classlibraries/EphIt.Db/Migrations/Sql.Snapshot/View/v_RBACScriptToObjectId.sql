CREATE OR ALTER VIEW v_RBACScriptToObjectId
AS
SELECT 
	rsr.ScriptId
	,mu.RoleId
	,mu.UserId as UserGroupId
	,1 AS 'IsUser'
	,ruo.ObjectId
FROM v_RBACScriptRole rsr
JOIN RoleMembershipUser mu ON mu.RoleId = rsr.RoleId
FULL JOIN v_RBACUserObjectId ruo ON ruo.UserId = mu.UserId
