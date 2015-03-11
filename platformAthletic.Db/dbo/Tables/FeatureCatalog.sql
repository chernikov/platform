CREATE TABLE [dbo].[FeatureCatalog] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Header]  NVARCHAR (500) NOT NULL,
    [OrderBy] INT            NOT NULL,
    CONSTRAINT [PK_FeatureCatalog] PRIMARY KEY CLUSTERED ([ID] ASC)
);

