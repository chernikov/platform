

ALTER TABLE [dbo].[UserSeason]
    ADD [StartFrom] INT DEFAULT (0) NOT NULL;
GO

ALTER TABLE [dbo].[Schedule]
    ADD [Date] DATETIME DEFAULT (GETDATE()) NOT NULL;
GO

ALTER TABLE [dbo].[PersonalSchedule]
    ADD [Date] DATETIME DEFAULT (GETDATE()) NOT NULL;
GO