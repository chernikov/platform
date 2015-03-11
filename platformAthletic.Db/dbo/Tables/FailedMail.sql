CREATE TABLE [dbo].[FailedMail] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [Subject]     NVARCHAR (MAX) NOT NULL,
    [Body]        NVARCHAR (MAX) NOT NULL,
    [IsProcessed] BIT            NOT NULL,
    [FailEmail]   NVARCHAR (512) NOT NULL,
    CONSTRAINT [PK_FailedMail] PRIMARY KEY CLUSTERED ([ID] ASC)
);

