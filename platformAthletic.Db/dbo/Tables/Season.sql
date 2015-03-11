CREATE TABLE [dbo].[Season] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Type] INT            NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Season] PRIMARY KEY CLUSTERED ([ID] ASC)
);

