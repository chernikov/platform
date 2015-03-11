CREATE TABLE [dbo].[Setting] (
    [ID]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (50)  NOT NULL,
    [Value] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([ID] ASC)
);

