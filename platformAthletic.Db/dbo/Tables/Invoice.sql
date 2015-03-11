CREATE TABLE [dbo].[Invoice] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [UserID]             INT            NOT NULL,
    [Code]               NVARCHAR (500) NOT NULL,
    [NameOfOrganization] NVARCHAR (500) NOT NULL,
    [City]               NVARCHAR (150) NOT NULL,
    [StateID]            INT            NOT NULL,
    [ZipCode]            NVARCHAR (50)  NOT NULL,
    [PhoneNumber]        NVARCHAR (50)  NULL,
    [DateSent]           DATETIME       NOT NULL,
    [DateDue]            DATETIME       NOT NULL,
    [TotalSum]           FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Invoice_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID]),
    CONSTRAINT [FK_Invoice_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

