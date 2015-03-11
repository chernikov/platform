CREATE TABLE [dbo].[TrainingEquipment] (
    [ID]            INT IDENTITY (1, 1) NOT NULL,
    [TrainingSetID] INT NOT NULL,
    [TrainingID]    INT NOT NULL,
    [EquipmentID]   INT NULL,
    [Equipment2ID]  INT NULL,
    [Priority]      INT NOT NULL,
    CONSTRAINT [PK_TrainingEquipment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TrainingEquipment_Equipment] FOREIGN KEY ([EquipmentID]) REFERENCES [dbo].[Equipment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TrainingEquipment_Equipment2] FOREIGN KEY ([Equipment2ID]) REFERENCES [dbo].[Equipment] ([ID]),
    CONSTRAINT [FK_TrainingEquipment_Training] FOREIGN KEY ([TrainingID]) REFERENCES [dbo].[Training] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TrainingEquipment_TrainingSet] FOREIGN KEY ([TrainingSetID]) REFERENCES [dbo].[TrainingSet] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

