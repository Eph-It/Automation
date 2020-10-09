CREATE TABLE [dbo].[Role_Object_Scope_Script]
(
	[Role_Object_Scope_Script_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Script_Id] INT NOT NULL, 
    [Role_Id] INT NOT NULL, 
    CONSTRAINT [FK_Role_Object_Scope_Script_Script] FOREIGN KEY ([Script_Id]) REFERENCES [Script]([Script_Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Role_Object_Scope_Script_Role] FOREIGN KEY ([Role_Id]) REFERENCES [Role]([Role_Id])
)
