CREATE TABLE [dbo].[Job]
(
	[Job_UId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Script_Id] INT NOT NULL, 
    [Job_Status_Id] SMALLINT NOT NULL, 
    [Created_By_User_Id] INT NOT NULL, 
    [Created] DATETIME NOT NULL, 
    [Finished] DATETIME NULL, 
    CONSTRAINT [FK_Job_Script] FOREIGN KEY ([Script_Id]) REFERENCES [Script]([Script_Id]), 
    CONSTRAINT [FK_Job_Job_Status] FOREIGN KEY ([Job_Status_Id]) REFERENCES [Job_Status]([Job_Status_Id]),
    CONSTRAINT [FK_Job_CreatedBy] FOREIGN KEY ([Created_By_User_Id]) REFERENCES [User]([User_Id])
)
