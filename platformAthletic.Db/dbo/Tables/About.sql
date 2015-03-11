CREATE TABLE [dbo].[About] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Text]   NVARCHAR (500) NOT NULL,
    [Author] NVARCHAR (100) NULL,
    CONSTRAINT [PK_About] PRIMARY KEY CLUSTERED ([ID] ASC)
);

