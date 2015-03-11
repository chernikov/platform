CREATE TABLE [dbo].[Aphorism] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Author] NVARCHAR (500) NOT NULL,
    [Text]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Aphorism] PRIMARY KEY CLUSTERED ([ID] ASC)
);

