CREATE TABLE [dbo].[Script]
(
	[Script_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Created_By_User_Id] INT NOT NULL,
    [Modified] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Modified_By_User_Id] INT NOT NULL, 
    [Published_Version] INT NULL, 
    [Name] NVARCHAR(150) NOT NULL, 
    [Description] NVARCHAR(200) NULL, 
    CONSTRAINT [FK_Script_User_CreatedBy] FOREIGN KEY ([Created_By_User_Id]) REFERENCES [User]([User_Id]),
    CONSTRAINT [FK_Script_User_ModifiedBy] FOREIGN KEY ([Modified_By_User_Id]) REFERENCES [User]([User_Id])
)
