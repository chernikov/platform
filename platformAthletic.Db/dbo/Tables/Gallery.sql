CREATE TABLE [dbo].[Gallery] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [ImagePath] NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_Gallery] PRIMARY KEY CLUSTERED ([ID] ASC)
);

