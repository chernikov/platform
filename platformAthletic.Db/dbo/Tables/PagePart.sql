﻿CREATE TABLE [dbo].[PagePart]
(
	[ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50)  NOT NULL,
    [Text] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PagePart] PRIMARY KEY CLUSTERED ([ID] ASC)
)
