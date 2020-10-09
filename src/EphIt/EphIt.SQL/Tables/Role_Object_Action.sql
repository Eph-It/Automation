CREATE TABLE [dbo].[Role_Object_Action]
(
	[Role_Object_Action_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RBAC_Action_Id] SMALLINT NOT NULL, 
    [RBAC_Object_Id] SMALLINT NOT NULL, 
    [Role_Id] INT NOT NULL, 
    CONSTRAINT [FK_Role_Object_Action_RBAC_Action] FOREIGN KEY ([RBAC_Action_Id]) REFERENCES [RBAC_Action]([RBAC_Action_Id]),
    CONSTRAINT [FK_Role_Object_Action_RBAC_Object] FOREIGN KEY ([RBAC_Object_Id]) REFERENCES [RBAC_Object]([RBAC_Object_Id]),
    CONSTRAINT [FK_Role_Object_Action_Role] FOREIGN KEY ([Role_Id]) REFERENCES [Role]([Role_Id])
)
