CREATE TABLE [dbo].[User_Windows]
(
	[User_Windows_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [User_Id] INT NOT NULL,
    [FirstName] NVARCHAR(64) NULL, 
    [LastName] NVARCHAR(100) NULL, 
    [UserName] NVARCHAR(256) NOT NULL, 
    [Domain] NVARCHAR(100) NOT NULL, 
    [Email] NVARCHAR(100) NULL, 
    [DisplayName] NVARCHAR(250) NULL, 
    [SID] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_User_Windows_User] FOREIGN KEY ([User_Id]) REFERENCES [User]([User_Id])
)
