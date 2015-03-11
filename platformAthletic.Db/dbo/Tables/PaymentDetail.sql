CREATE TABLE [dbo].[PaymentDetail] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [UserID]        INT            NOT NULL,
    [ReferralCode]  NVARCHAR (50)  NULL,
    [Amount]        FLOAT (53)     NOT NULL,
    [AddedDate]     DATETIME       NOT NULL,
    [Description]   NVARCHAR (MAX) NOT NULL,
    [ProcessedDate] DATETIME       NULL,
    [Result]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_PaymentDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PaymentDetail_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

