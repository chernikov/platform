CREATE TABLE [dbo].[PillarType] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (500) NOT NULL,
    [Measure]     NVARCHAR (50)  NOT NULL,
    [TextAbove]   NVARCHAR (500) NULL,
    [VideoUrl]    NVARCHAR (500) NULL,
    [VideoCode]   NVARCHAR (MAX) NULL,
	[Preview]    NVARCHAR (150) NULL DEFAULT '',
    [Type]        INT            NULL,
    [Placeholder] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_PillarType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

