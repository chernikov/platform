CREATE TABLE [dbo].[TrainingDay] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [WeekID]       INT NOT NULL,
    [MacrocycleID] INT NULL,
    [DayID]        INT NOT NULL,
    CONSTRAINT [PK_TrainingDay] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TrainingDay_Day] FOREIGN KEY ([DayID]) REFERENCES [dbo].[Day] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TrainingDay_Macrocycle] FOREIGN KEY ([MacrocycleID]) REFERENCES [dbo].[Macrocycle] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TrainingDay_Week] FOREIGN KEY ([WeekID]) REFERENCES [dbo].[Week] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

