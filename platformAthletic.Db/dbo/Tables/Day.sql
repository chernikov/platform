﻿CREATE TABLE [dbo].[Day] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Day] PRIMARY KEY CLUSTERED ([ID] ASC)
);

