CREATE TABLE [dbo].[Feedback] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (500) NOT NULL,
    [Email]     NVARCHAR (500) NOT NULL,
    [Phone]     NVARCHAR (50)  NULL,
    [School]    NVARCHAR (500) NOT NULL,
    [City]      NVARCHAR (500) NOT NULL,
    [StateID]   INT            NOT NULL,
    [Message]   NVARCHAR (MAX) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    [IsReaded]  BIT            NOT NULL,
    CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Feedback_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID])
);

