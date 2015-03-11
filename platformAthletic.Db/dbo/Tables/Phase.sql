CREATE TABLE [dbo].[Phase] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [CycleID] INT            NOT NULL,
    [Name]    NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Phase] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Phase_Cycle] FOREIGN KEY ([CycleID]) REFERENCES [dbo].[Cycle] ([ID])
);

