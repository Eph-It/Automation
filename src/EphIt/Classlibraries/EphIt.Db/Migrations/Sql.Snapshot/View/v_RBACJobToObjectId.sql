CREATE OR ALTER VIEW v_RBACJobToObjectId
AS
SELECT 
	jr.ScriptId
	,jr.ScriptVersionId
	,jr.JobUid
	,mu.RoleId
	,mu.UserId as UserGroupId
	,1 AS 'IsUser'
	,ruo.ObjectId
FROM v_RBACJobRole jr
JOIN RoleMembershipUser mu ON mu.RoleId = jr.RoleId
FULL JOIN v_RBACUserObjectId ruo ON ruo.UserId = mu.UserId