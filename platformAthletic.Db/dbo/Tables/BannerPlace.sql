CREATE TABLE [dbo].[BannerPlace] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (500) NOT NULL,
    [Height] INT            NOT NULL,
    [Width]  INT            NOT NULL,
    CONSTRAINT [PK_BannerPlace] PRIMARY KEY CLUSTERED ([ID] ASC)
);

