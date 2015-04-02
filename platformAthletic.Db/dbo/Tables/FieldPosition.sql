CREATE TABLE [dbo].[FieldPosition] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
	[SportID] INT NULL,
    [Code] NVARCHAR (50)  NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_FieldPosition] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_FieldPosition_Sport] FOREIGN KEY ([SportID]) REFERENCES [dbo].[Sport] ([ID])
);

