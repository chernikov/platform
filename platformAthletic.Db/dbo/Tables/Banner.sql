CREATE TABLE [dbo].[Banner] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [BannerPlaceID] INT            NOT NULL,
    [Name]          NVARCHAR (500) NOT NULL,
    [Code]          NVARCHAR (MAX) NULL,
    [SourcePath]    NVARCHAR (150) NULL,
    [ImagePath]     NVARCHAR (150) NULL,
    [Link]          NVARCHAR (500) NULL,
    [InRotation]    BIT            NOT NULL,
    CONSTRAINT [PK_Banner] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Banner_BannerPlace] FOREIGN KEY ([BannerPlaceID]) REFERENCES [dbo].[BannerPlace] ([ID])
);

