CREATE TABLE [dbo].[PeopleSaying]
(
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Author] NVARCHAR (500) NOT NULL,
	[ImagePath] NVARCHAR (150) NOT NULL,
    [Text]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PeopleSaying] PRIMARY KEY CLUSTERED ([ID] ASC)
);