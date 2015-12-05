﻿CREATE TABLE [dbo].[UserSeason] (
    [ID]       INT      IDENTITY (1, 1) NOT NULL,
    [SeasonID] INT      NOT NULL,
    [UserID]   INT      NOT NULL,
	[GroupID]   INT     NULL,
    [StartDay] DATETIME NOT NULL,
	[StartFrom] INT NOT NULL DEFAULT(0),
    CONSTRAINT [PK_UserSeason] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserSeason_Season] FOREIGN KEY ([SeasonID]) REFERENCES [dbo].[Season] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserSeason_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]),
	CONSTRAINT [FK_UserSeason_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE NO ACTION ON UPDATE  NO ACTION
);

