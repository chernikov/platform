CREATE TABLE [dbo].[Macrocycle] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [WeekID] INT            NOT NULL,
    [Name]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Macrocycle] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Macrocycle_Week] FOREIGN KEY ([WeekID]) REFERENCES [dbo].[Week] ([ID])
);

