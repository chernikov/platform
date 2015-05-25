CREATE TABLE [dbo].[Post] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [Header]    NVARCHAR (50)  NOT NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    [TitleImagePath] NVARCHAR(150) NOT NULL DEFAULT '', 
    [Promoted] BIT NOT NULL DEFAULT 0, 
    [IsVideo] BIT NOT NULL DEFAULT 0, 
    [VideoUrl] NVARCHAR(150) NULL, 
	[VideoCode]  NVARCHAR (MAX) NULL,
    [VideoPreview]    NVARCHAR (150)  NULL,
	[CountOfView] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Post_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

