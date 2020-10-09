CREATE TABLE [dbo].[Script_Language]
(
	[Script_Language_Id] SMALLINT NOT NULL PRIMARY KEY, 
    [Language] NVARCHAR(50) NOT NULL, 
    [Version] NVARCHAR(20) NULL
)
