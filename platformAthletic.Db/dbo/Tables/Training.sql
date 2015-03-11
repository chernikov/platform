CREATE TABLE [dbo].[Training] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Training] PRIMARY KEY CLUSTERED ([ID] ASC)
);

