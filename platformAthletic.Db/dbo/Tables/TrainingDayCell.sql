CREATE TABLE [dbo].[TrainingDayCell] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [CellID]        INT           NOT NULL,
    [TrainingDayID] INT           NOT NULL,
    [PrimaryText]   NVARCHAR (50) NULL,
    [TrainingSetID] INT           NULL,
    [SBCType]       INT           NULL,
    [Coefficient]   FLOAT (53)    NULL,
    CONSTRAINT [PK_TrainingDayCell] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TrainingDayCell_Cell] FOREIGN KEY ([CellID]) REFERENCES [dbo].[Cell] ([ID]),
    CONSTRAINT [FK_TrainingDayCell_TrainingDay] FOREIGN KEY ([TrainingDayID]) REFERENCES [dbo].[TrainingDay] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TrainingDayCell_TrainingSet] FOREIGN KEY ([TrainingSetID]) REFERENCES [dbo].[TrainingSet] ([ID])
);

