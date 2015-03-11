CREATE TABLE [dbo].[SBCValue] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          INT            NULL,
    [FieldPositionID] INT            NULL,
    [TeamID]          INT            NULL,
    [AddedDate]       DATETIME       NOT NULL,
    [Squat]           FLOAT (53)     NOT NULL,
    [Bench]           FLOAT (53)     NOT NULL,
    [Clean]           FLOAT (53)     NOT NULL,
    [FirstName]       NVARCHAR (500) NULL,
    [LastName]        NVARCHAR (500) NULL,
    CONSTRAINT [PK_SBCValue] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SBCValue_FieldPosition] FOREIGN KEY ([FieldPositionID]) REFERENCES [dbo].[FieldPosition] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SBCValue_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]),
    CONSTRAINT [FK_SBCValue_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

