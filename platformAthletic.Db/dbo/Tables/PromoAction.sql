CREATE TABLE [dbo].[PromoAction] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (500) NOT NULL,
    [Type]      INT            NOT NULL,
    [Target]    INT            NOT NULL,
    [Amount]    FLOAT (53)     NOT NULL,
    [ValidDate] DATETIME       NULL,
    [Closed]    BIT            NOT NULL,
    [Reusable]  BIT            NOT NULL,
    CONSTRAINT [PK_PromoAction] PRIMARY KEY CLUSTERED ([ID] ASC)
);

