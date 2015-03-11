CREATE TABLE [dbo].[State] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    [Code] NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([ID] ASC)
);

