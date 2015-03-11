CREATE TABLE [dbo].[BillingInfo] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [UserID]           INT            NOT NULL,
    [AutoDebit]        BIT            CONSTRAINT [DF_BillingInfo_AutoDebit] DEFAULT ((1)) NOT NULL,
    [NameOnCard]       NVARCHAR (500) NOT NULL,
    [BillingAddress]   NVARCHAR (MAX) NOT NULL,
    [City]             NVARCHAR (50)  NOT NULL,
    [StateID]          INT            NOT NULL,
    [ZipCode]          NVARCHAR (50)  NOT NULL,
    [CardType]         INT            NOT NULL,
    [CreditCardNumber] NVARCHAR (50)  NOT NULL,
    [ExparationDate]   DATETIME       NOT NULL,
    [CVC]              NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_BillingInfo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BillingInfo_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID]),
    CONSTRAINT [FK_BillingInfo_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

