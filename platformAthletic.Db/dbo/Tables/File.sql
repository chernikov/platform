CREATE TABLE [dbo].[File] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Path]      NVARCHAR (150) NOT NULL,
    [Preview]   NVARCHAR (150) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED ([ID] ASC)
);

