CREATE TABLE [dbo].[UserFieldPosition]
(
	[ID]		INT IDENTITY (1, 1) NOT NULL,
	[SportID]	INT NOT NULL,
	[UserID]	INT NOT NULL,
	[FieldPositionID] INT NULL,
	CONSTRAINT [PK_UserFieldPosition] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_UserFieldPosition_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]),
	CONSTRAINT [FK_UserFieldPosition_Sport] FOREIGN KEY ([SportID]) REFERENCES [dbo].[Sport] ([ID]),
	CONSTRAINT [FK_UserFieldPosition_FieldPosition] FOREIGN KEY ([FieldPositionID]) REFERENCES [dbo].[FieldPosition] ([ID])
)
