CREATE TABLE [dbo].[SBCValue] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          INT            NOT NULL,
    [TeamID]          INT            NULL,
    [AddedDate]       DATETIME       NOT NULL,
    [Squat]           FLOAT (53)     NOT NULL,
    [Bench]           FLOAT (53)     NOT NULL,
    [Clean]           FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_SBCValue] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SBCValue_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]),
    CONSTRAINT [FK_SBCValue_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

