CREATE TABLE [dbo].[UserEquipment] (
    [ID]          INT IDENTITY (1, 1) NOT NULL,
    [UserID]      INT NOT NULL,
    [EquipmentID] INT NOT NULL,
    CONSTRAINT [PK_UserEquipment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserEquipment_Equipment] FOREIGN KEY ([EquipmentID]) REFERENCES [dbo].[Equipment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserEquipment_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

