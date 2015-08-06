CREATE TABLE [dbo].[PersonalSchedule] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [UserSeasonID] INT NULL,
    [UserID]       INT NOT NULL,
    [Number]       INT NOT NULL,
    [MacrocycleID] INT NOT NULL,
    CONSTRAINT [PK_PersonalSchedule] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PersonalSchedule_Macrocycle] FOREIGN KEY ([MacrocycleID]) REFERENCES [dbo].[Macrocycle] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PersonalSchedule_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PersonalSchedule_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
);

