CREATE TABLE [dbo].[Schedule] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [UserSeasonID] INT NULL,
    [TeamID]       INT NULL,
    [GroupID]      INT NULL,
    [Number]       INT NOT NULL,
    [MacrocycleID] INT NOT NULL,
	[Date]         DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Schedule_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Schedule_Macrocycle] FOREIGN KEY ([MacrocycleID]) REFERENCES [dbo].[Macrocycle] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Schedule_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]),
    CONSTRAINT [FK_Schedule_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID])
);

