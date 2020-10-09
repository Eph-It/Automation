CREATE TABLE [dbo].[Role]
(
	[Role_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Created_By_User_Id] INT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Modified_By_User_Id] INT NULL, 
    [Modified] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Description] NVARCHAR(200) NULL, 
    [IsGlobal] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Role_User_CreatedBy] FOREIGN KEY ([Created_By_User_Id]) REFERENCES [User]([User_Id]),
    CONSTRAINT [FK_Role_User_ModifiedBy] FOREIGN KEY ([Modified_By_User_Id]) REFERENCES [User]([User_Id])
)
