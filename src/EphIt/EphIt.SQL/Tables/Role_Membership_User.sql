CREATE TABLE [dbo].[Role_Membership_User]
(
	[Role_Membership_User_Id] INT NOT NULL PRIMARY KEY, 
    [Role_Id] INT NOT NULL, 
    [User_Id] INT NOT NULL, 
    CONSTRAINT [FK_Role_Membership_User_Role] FOREIGN KEY ([Role_Id]) REFERENCES [Role]([Role_Id]),
    CONSTRAINT [FK_Role_Membership_User_User] FOREIGN KEY ([User_Id]) REFERENCES [User]([User_Id])
)
