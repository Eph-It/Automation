CREATE TABLE [dbo].[User]
(
	[User_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Authentication_Id] SMALLINT NOT NULL, 
    CONSTRAINT [FK_User_Authentication] FOREIGN KEY ([Authentication_Id]) REFERENCES [Authentication]([Authentication_Id])
)
