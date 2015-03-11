CREATE TABLE [dbo].[Cell] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Type] INT           NOT NULL,
    [Name] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Cell] PRIMARY KEY CLUSTERED ([ID] ASC)
);

