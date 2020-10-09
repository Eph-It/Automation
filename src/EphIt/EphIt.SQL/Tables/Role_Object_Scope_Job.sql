CREATE TABLE [dbo].[Role_Object_Scope_Job]
(
	[Role_Object_Scope_Job_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Role_Id] INT NOT NULL, 
    [Script_Id] INT NOT NULL, 
    CONSTRAINT [FK_Role_Object_Scope_Job_Role] FOREIGN KEY ([Role_Id]) REFERENCES [Role]([Role_Id]),
    CONSTRAINT [FK_Role_Object_Scope_Job_Script] FOREIGN KEY ([Script_Id]) REFERENCES [Script]([Script_Id])
)
