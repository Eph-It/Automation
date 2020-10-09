CREATE TABLE [dbo].[Script_Version]
(
	[Script_Version_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Body] NVARCHAR(MAX) NOT NULL, 
    [Version] INT NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Created_By_User_Id] INT NOT NULL,
    [Script_Language_Id] SMALLINT NULL, 
    [Script_Id] INT NOT NULL, 
    CONSTRAINT [FK_Script_Version_User_CreatedBy] FOREIGN KEY ([Created_By_User_Id]) REFERENCES [User]([User_Id]), 
    CONSTRAINT [FK_Script_Version_Language] FOREIGN KEY ([Script_Language_Id]) REFERENCES [Script_Language]([Script_Language_Id]),
    CONSTRAINT [FK_Script_Version_Script] FOREIGN KEY ([Script_Id]) REFERENCES [Script]([Script_Id])
)
