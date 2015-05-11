CREATE TABLE [dbo].[UserVideo]
(
	[ID]         INT            IDENTITY (1, 1) NOT NULL,
	[UserID]     INT NOT NULL,
    [Header]     NVARCHAR (500) NOT NULL,
    [VideoUrl]   NVARCHAR (500) NOT NULL,
    [VideoCode]  NVARCHAR (MAX) NOT NULL,
    [Preview]    NVARCHAR(150) NOT NULL DEFAULT '', 
    CONSTRAINT [PK_UserVideo] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_UserVideo_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
)
