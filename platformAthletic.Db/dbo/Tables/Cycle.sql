CREATE TABLE [dbo].[Cycle] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [SeasonID] INT            NOT NULL,
    [Name]     NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Cycle] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Cycle_Season] FOREIGN KEY ([SeasonID]) REFERENCES [dbo].[Season] ([ID])
);

