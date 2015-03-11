CREATE TABLE [dbo].[UserPillar] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [UserID]       INT           NOT NULL,
    [PillarTypeID] INT           NOT NULL,
    [Value]        INT           NOT NULL,
    [AddedDate]    DATETIME      NULL,
    [TextValue]    NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_UserPillar] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserPillar_PillarType] FOREIGN KEY ([PillarTypeID]) REFERENCES [dbo].[PillarType] ([ID]),
    CONSTRAINT [FK_UserPillar_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

