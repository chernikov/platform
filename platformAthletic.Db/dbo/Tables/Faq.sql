CREATE TABLE [dbo].[Faq] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Header]  NVARCHAR (500) NOT NULL,
    [Text]    NVARCHAR (MAX) NOT NULL,
    [OrderBy] INT            NOT NULL,
    CONSTRAINT [PK_Faq] PRIMARY KEY CLUSTERED ([ID] ASC)
);

