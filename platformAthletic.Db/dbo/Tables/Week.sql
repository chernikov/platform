CREATE TABLE [dbo].[Week] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [PhaseID] INT            NOT NULL,
    [Number]  INT            NULL,
    [Name]    NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Week] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Week_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

