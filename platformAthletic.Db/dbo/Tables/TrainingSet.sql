CREATE TABLE [dbo].[TrainingSet] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [DayID]   INT NOT NULL,
    [PhaseID] INT NOT NULL,
    CONSTRAINT [PK_TrainingSet] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TrainingSet_Day] FOREIGN KEY ([DayID]) REFERENCES [dbo].[Day] ([ID]),
    CONSTRAINT [FK_TrainingSet_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID])
);

