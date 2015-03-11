CREATE TABLE [dbo].[FeatureText] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [FeatureCatalogID] INT            NOT NULL,
    [OrderBy]          INT            NOT NULL,
    [Header]           NVARCHAR (500) NOT NULL,
    [Text]             NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_FeatureText] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FeatureText_FeatureCatalog] FOREIGN KEY ([FeatureCatalogID]) REFERENCES [dbo].[FeatureCatalog] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

