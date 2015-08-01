

GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_About] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Text]   NVARCHAR (500) NOT NULL,
    [Author] NVARCHAR (100) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_About] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[About])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_About] ON;
        INSERT INTO [dbo].[tmp_ms_xx_About] ([ID], [Text], [Author])
        SELECT   [ID],
                 [Text],
                 [Author]
        FROM     [dbo].[About]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_About] OFF;
    END

DROP TABLE [dbo].[About];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_About]', N'About';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_About]', N'PK_About', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Aphorism]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Aphorism] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Author] NVARCHAR (500) NOT NULL,
    [Text]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Aphorism] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Aphorism])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Aphorism] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Aphorism] ([ID], [Author], [Text])
        SELECT   [ID],
                 [Author],
                 [Text]
        FROM     [dbo].[Aphorism]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Aphorism] OFF;
    END

DROP TABLE [dbo].[Aphorism];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Aphorism]', N'Aphorism';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Aphorism]', N'PK_Aphorism', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Banner]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Banner] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [BannerPlaceID] INT            NOT NULL,
    [Name]          NVARCHAR (500) NOT NULL,
    [Code]          NVARCHAR (MAX) NULL,
    [SourcePath]    NVARCHAR (150) NULL,
    [ImagePath]     NVARCHAR (150) NULL,
    [Link]          NVARCHAR (500) NULL,
    [InRotation]    BIT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Banner] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Banner])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Banner] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Banner] ([ID], [BannerPlaceID], [Name], [Code], [SourcePath], [ImagePath], [Link], [InRotation])
        SELECT   [ID],
                 [BannerPlaceID],
                 [Name],
                 [Code],
                 [SourcePath],
                 [ImagePath],
                 [Link],
                 [InRotation]
        FROM     [dbo].[Banner]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Banner] OFF;
    END

DROP TABLE [dbo].[Banner];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Banner]', N'Banner';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Banner]', N'PK_Banner', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[BannerPlace]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_BannerPlace] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (500) NOT NULL,
    [Height] INT            NOT NULL,
    [Width]  INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_BannerPlace] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[BannerPlace])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_BannerPlace] ON;
        INSERT INTO [dbo].[tmp_ms_xx_BannerPlace] ([ID], [Name], [Height], [Width])
        SELECT   [ID],
                 [Name],
                 [Height],
                 [Width]
        FROM     [dbo].[BannerPlace]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_BannerPlace] OFF;
    END

DROP TABLE [dbo].[BannerPlace];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_BannerPlace]', N'BannerPlace';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_BannerPlace]', N'PK_BannerPlace', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[BillingInfo]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_BillingInfo] (
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
    CONSTRAINT [tmp_ms_xx_constraint_PK_BillingInfo] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[BillingInfo])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_BillingInfo] ON;
        INSERT INTO [dbo].[tmp_ms_xx_BillingInfo] ([ID], [UserID], [AutoDebit], [NameOnCard], [BillingAddress], [City], [StateID], [ZipCode], [CardType], [CreditCardNumber], [ExparationDate], [CVC])
        SELECT   [ID],
                 [UserID],
                 [AutoDebit],
                 [NameOnCard],
                 [BillingAddress],
                 [City],
                 [StateID],
                 [ZipCode],
                 [CardType],
                 [CreditCardNumber],
                 [ExparationDate],
                 [CVC]
        FROM     [dbo].[BillingInfo]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_BillingInfo] OFF;
    END

DROP TABLE [dbo].[BillingInfo];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_BillingInfo]', N'BillingInfo';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_BillingInfo]', N'PK_BillingInfo', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Cell]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Cell] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Type] INT           NOT NULL,
    [Name] NVARCHAR (50) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Cell] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Cell])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Cell] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Cell] ([ID], [Type], [Name])
        SELECT   [ID],
                 [Type],
                 [Name]
        FROM     [dbo].[Cell]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Cell] OFF;
    END

DROP TABLE [dbo].[Cell];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Cell]', N'Cell';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Cell]', N'PK_Cell', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Cycle]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Cycle] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [SeasonID] INT            NOT NULL,
    [Name]     NVARCHAR (500) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Cycle] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Cycle])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Cycle] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Cycle] ([ID], [SeasonID], [Name])
        SELECT   [ID],
                 [SeasonID],
                 [Name]
        FROM     [dbo].[Cycle]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Cycle] OFF;
    END

DROP TABLE [dbo].[Cycle];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Cycle]', N'Cycle';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Cycle]', N'PK_Cycle', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Day]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Day] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Day] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Day])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Day] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Day] ([ID], [Name])
        SELECT   [ID],
                 [Name]
        FROM     [dbo].[Day]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Day] OFF;
    END

DROP TABLE [dbo].[Day];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Day]', N'Day';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Day]', N'PK_Day', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Equipment]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Equipment] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (500) NOT NULL,
    [ImagePath] NVARCHAR (150) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Equipment] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Equipment])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Equipment] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Equipment] ([ID], [Name], [ImagePath])
        SELECT   [ID],
                 [Name],
                 [ImagePath]
        FROM     [dbo].[Equipment]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Equipment] OFF;
    END

DROP TABLE [dbo].[Equipment];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Equipment]', N'Equipment';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Equipment]', N'PK_Equipment', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[FailedMail]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_FailedMail] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [Subject]     NVARCHAR (MAX) NOT NULL,
    [Body]        NVARCHAR (MAX) NOT NULL,
    [IsProcessed] BIT            NOT NULL,
    [FailEmail]   NVARCHAR (512) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_FailedMail] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[FailedMail])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FailedMail] ON;
        INSERT INTO [dbo].[tmp_ms_xx_FailedMail] ([ID], [AddedDate], [Subject], [Body], [IsProcessed], [FailEmail])
        SELECT   [ID],
                 [AddedDate],
                 [Subject],
                 [Body],
                 [IsProcessed],
                 [FailEmail]
        FROM     [dbo].[FailedMail]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FailedMail] OFF;
    END

DROP TABLE [dbo].[FailedMail];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_FailedMail]', N'FailedMail';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_FailedMail]', N'PK_FailedMail', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Faq]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Faq] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Header]  NVARCHAR (500) NOT NULL,
    [Text]    NVARCHAR (MAX) NOT NULL,
    [OrderBy] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Faq] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Faq])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Faq] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Faq] ([ID], [Header], [Text], [OrderBy])
        SELECT   [ID],
                 [Header],
                 [Text],
                 [OrderBy]
        FROM     [dbo].[Faq]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Faq] OFF;
    END

DROP TABLE [dbo].[Faq];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Faq]', N'Faq';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Faq]', N'PK_Faq', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[FeatureCatalog]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_FeatureCatalog] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Header]  NVARCHAR (500) NOT NULL,
    [OrderBy] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_FeatureCatalog] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[FeatureCatalog])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FeatureCatalog] ON;
        INSERT INTO [dbo].[tmp_ms_xx_FeatureCatalog] ([ID], [Header], [OrderBy])
        SELECT   [ID],
                 [Header],
                 [OrderBy]
        FROM     [dbo].[FeatureCatalog]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FeatureCatalog] OFF;
    END

DROP TABLE [dbo].[FeatureCatalog];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_FeatureCatalog]', N'FeatureCatalog';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_FeatureCatalog]', N'PK_FeatureCatalog', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[FeatureText]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_FeatureText] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [FeatureCatalogID] INT            NOT NULL,
    [OrderBy]          INT            NOT NULL,
    [Header]           NVARCHAR (500) NOT NULL,
    [Text]             NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_FeatureText] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[FeatureText])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FeatureText] ON;
        INSERT INTO [dbo].[tmp_ms_xx_FeatureText] ([ID], [FeatureCatalogID], [OrderBy], [Header], [Text])
        SELECT   [ID],
                 [FeatureCatalogID],
                 [OrderBy],
                 [Header],
                 [Text]
        FROM     [dbo].[FeatureText]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FeatureText] OFF;
    END

DROP TABLE [dbo].[FeatureText];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_FeatureText]', N'FeatureText';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_FeatureText]', N'PK_FeatureText', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Feedback]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Feedback] (
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
    CONSTRAINT [tmp_ms_xx_constraint_PK_Feedback] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Feedback])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Feedback] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Feedback] ([ID], [Name], [Email], [Phone], [School], [City], [StateID], [Message], [AddedDate], [IsReaded])
        SELECT   [ID],
                 [Name],
                 [Email],
                 [Phone],
                 [School],
                 [City],
                 [StateID],
                 [Message],
                 [AddedDate],
                 [IsReaded]
        FROM     [dbo].[Feedback]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Feedback] OFF;
    END

DROP TABLE [dbo].[Feedback];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Feedback]', N'Feedback';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Feedback]', N'PK_Feedback', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[FieldPosition]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_FieldPosition] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Code] NVARCHAR (50)  NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_FieldPosition] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[FieldPosition])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FieldPosition] ON;
        INSERT INTO [dbo].[tmp_ms_xx_FieldPosition] ([ID], [Code], [Name])
        SELECT   [ID],
                 [Code],
                 [Name]
        FROM     [dbo].[FieldPosition]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FieldPosition] OFF;
    END

DROP TABLE [dbo].[FieldPosition];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_FieldPosition]', N'FieldPosition';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_FieldPosition]', N'PK_FieldPosition', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[File]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_File] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Path]      NVARCHAR (150) NOT NULL,
    [Preview]   NVARCHAR (150) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_File] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[File])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_File] ON;
        INSERT INTO [dbo].[tmp_ms_xx_File] ([ID], [Path], [Preview], [AddedDate])
        SELECT   [ID],
                 [Path],
                 [Preview],
                 [AddedDate]
        FROM     [dbo].[File]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_File] OFF;
    END

DROP TABLE [dbo].[File];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_File]', N'File';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_File]', N'PK_File', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Gallery]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Gallery] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [ImagePath] NVARCHAR (150) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Gallery] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Gallery])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Gallery] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Gallery] ([ID], [ImagePath])
        SELECT   [ID],
                 [ImagePath]
        FROM     [dbo].[Gallery]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Gallery] OFF;
    END

DROP TABLE [dbo].[Gallery];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Gallery]', N'Gallery';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Gallery]', N'PK_Gallery', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Group]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Group] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [TeamID] INT            NOT NULL,
    [Name]   NVARCHAR (500) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Group] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Group])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Group] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Group] ([ID], [TeamID], [Name])
        SELECT   [ID],
                 [TeamID],
                 [Name]
        FROM     [dbo].[Group]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Group] OFF;
    END

DROP TABLE [dbo].[Group];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Group]', N'Group';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Group]', N'PK_Group', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Invoice]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Invoice] (
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
    CONSTRAINT [tmp_ms_xx_constraint_PK_Invoice] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Invoice])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Invoice] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Invoice] ([ID], [UserID], [Code], [NameOfOrganization], [City], [StateID], [ZipCode], [PhoneNumber], [DateSent], [DateDue], [TotalSum])
        SELECT   [ID],
                 [UserID],
                 [Code],
                 [NameOfOrganization],
                 [City],
                 [StateID],
                 [ZipCode],
                 [PhoneNumber],
                 [DateSent],
                 [DateDue],
                 [TotalSum]
        FROM     [dbo].[Invoice]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Invoice] OFF;
    END

DROP TABLE [dbo].[Invoice];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Invoice]', N'Invoice';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Invoice]', N'PK_Invoice', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Macrocycle]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Macrocycle] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [WeekID] INT            NOT NULL,
    [Name]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Macrocycle] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Macrocycle])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Macrocycle] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Macrocycle] ([ID], [WeekID], [Name])
        SELECT   [ID],
                 [WeekID],
                 [Name]
        FROM     [dbo].[Macrocycle]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Macrocycle] OFF;
    END

DROP TABLE [dbo].[Macrocycle];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Macrocycle]', N'Macrocycle';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Macrocycle]', N'PK_Macrocycle', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Page]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Page] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50)  NOT NULL,
    [Text] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Page] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Page])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Page] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Page] ([ID], [Name], [Text])
        SELECT   [ID],
                 [Name],
                 [Text]
        FROM     [dbo].[Page]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Page] OFF;
    END

DROP TABLE [dbo].[Page];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Page]', N'Page';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Page]', N'PK_Page', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PagePart]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PagePart] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50)  NOT NULL,
    [Text] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PagePart] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PagePart])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PagePart] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PagePart] ([ID], [Name], [Text])
        SELECT   [ID],
                 [Name],
                 [Text]
        FROM     [dbo].[PagePart]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PagePart] OFF;
    END

DROP TABLE [dbo].[PagePart];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PagePart]', N'PagePart';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PagePart]', N'PK_PagePart', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PaymentDetail]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PaymentDetail] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [UserID]        INT            NOT NULL,
    [ReferralCode]  NVARCHAR (50)  NULL,
    [Amount]        FLOAT (53)     NOT NULL,
    [AddedDate]     DATETIME       NOT NULL,
    [Description]   NVARCHAR (MAX) NOT NULL,
    [ProcessedDate] DATETIME       NULL,
    [Result]        NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PaymentDetail] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PaymentDetail])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PaymentDetail] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PaymentDetail] ([ID], [UserID], [ReferralCode], [Amount], [AddedDate], [Description], [ProcessedDate], [Result])
        SELECT   [ID],
                 [UserID],
                 [ReferralCode],
                 [Amount],
                 [AddedDate],
                 [Description],
                 [ProcessedDate],
                 [Result]
        FROM     [dbo].[PaymentDetail]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PaymentDetail] OFF;
    END

DROP TABLE [dbo].[PaymentDetail];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PaymentDetail]', N'PaymentDetail';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PaymentDetail]', N'PK_PaymentDetail', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PeopleSaying]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PeopleSaying] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Author]    NVARCHAR (500) NOT NULL,
    [ImagePath] NVARCHAR (150) NOT NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PeopleSaying] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PeopleSaying])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PeopleSaying] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PeopleSaying] ([ID], [Author], [ImagePath], [Text])
        SELECT   [ID],
                 [Author],
                 [ImagePath],
                 [Text]
        FROM     [dbo].[PeopleSaying]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PeopleSaying] OFF;
    END

DROP TABLE [dbo].[PeopleSaying];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PeopleSaying]', N'PeopleSaying';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PeopleSaying]', N'PK_PeopleSaying', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PersonalSchedule]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PersonalSchedule] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [UserSeasonID] INT NULL,
    [UserID]       INT NOT NULL,
    [Number]       INT NOT NULL,
    [MacrocycleID] INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PersonalSchedule] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PersonalSchedule])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PersonalSchedule] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PersonalSchedule] ([ID], [UserSeasonID], [UserID], [Number], [MacrocycleID])
        SELECT   [ID],
                 [UserSeasonID],
                 [UserID],
                 [Number],
                 [MacrocycleID]
        FROM     [dbo].[PersonalSchedule]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PersonalSchedule] OFF;
    END

DROP TABLE [dbo].[PersonalSchedule];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PersonalSchedule]', N'PersonalSchedule';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PersonalSchedule]', N'PK_PersonalSchedule', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Phase]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Phase] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [CycleID] INT            NOT NULL,
    [Name]    NVARCHAR (500) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Phase] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Phase])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Phase] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Phase] ([ID], [CycleID], [Name])
        SELECT   [ID],
                 [CycleID],
                 [Name]
        FROM     [dbo].[Phase]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Phase] OFF;
    END

DROP TABLE [dbo].[Phase];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Phase]', N'Phase';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Phase]', N'PK_Phase', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PillarType]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PillarType] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (500) NOT NULL,
    [Measure]     NVARCHAR (50)  NOT NULL,
    [TextAbove]   NVARCHAR (500) NULL,
    [VideoUrl]    NVARCHAR (500) NULL,
    [VideoCode]   NVARCHAR (MAX) NULL,
    [Type]        INT            NULL,
    [Placeholder] NVARCHAR (50)  NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PillarType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PillarType])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PillarType] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PillarType] ([ID], [Name], [Measure], [TextAbove], [VideoUrl], [VideoCode], [Type], [Placeholder])
        SELECT   [ID],
                 [Name],
                 [Measure],
                 [TextAbove],
                 [VideoUrl],
                 [VideoCode],
                 [Type],
                 [Placeholder]
        FROM     [dbo].[PillarType]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PillarType] OFF;
    END

DROP TABLE [dbo].[PillarType];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PillarType]', N'PillarType';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PillarType]', N'PK_PillarType', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Post]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Post] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [Header]    NVARCHAR (50)  NOT NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Post] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Post])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Post] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Post] ([ID], [UserID], [Header], [Text], [AddedDate])
        SELECT   [ID],
                 [UserID],
                 [Header],
                 [Text],
                 [AddedDate]
        FROM     [dbo].[Post]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Post] OFF;
    END

DROP TABLE [dbo].[Post];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Post]', N'Post';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Post]', N'PK_Post', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PromoAction]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PromoAction] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (500) NOT NULL,
    [Type]      INT            NOT NULL,
    [Target]    INT            NOT NULL,
    [Amount]    FLOAT (53)     NOT NULL,
    [ValidDate] DATETIME       NULL,
    [Closed]    BIT            NOT NULL,
    [Reusable]  BIT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PromoAction] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PromoAction])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PromoAction] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PromoAction] ([ID], [Name], [Type], [Target], [Amount], [ValidDate], [Closed], [Reusable])
        SELECT   [ID],
                 [Name],
                 [Type],
                 [Target],
                 [Amount],
                 [ValidDate],
                 [Closed],
                 [Reusable]
        FROM     [dbo].[PromoAction]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PromoAction] OFF;
    END

DROP TABLE [dbo].[PromoAction];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PromoAction]', N'PromoAction';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PromoAction]', N'PK_PromoAction', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PromoCode]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PromoCode] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [PromoActionID] INT           NOT NULL,
    [ReferralCode]  NVARCHAR (50) NOT NULL,
    [AddedDate]     DATETIME      NOT NULL,
    [UsedDate]      DATETIME      NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PromoCode] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PromoCode])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PromoCode] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PromoCode] ([ID], [PromoActionID], [ReferralCode], [AddedDate], [UsedDate])
        SELECT   [ID],
                 [PromoActionID],
                 [ReferralCode],
                 [AddedDate],
                 [UsedDate]
        FROM     [dbo].[PromoCode]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PromoCode] OFF;
    END

DROP TABLE [dbo].[PromoCode];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PromoCode]', N'PromoCode';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PromoCode]', N'PK_PromoCode', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Role]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Role] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Code] NVARCHAR (50) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Role] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Role])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Role] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Role] ([ID], [Code], [Name])
        SELECT   [ID],
                 [Code],
                 [Name]
        FROM     [dbo].[Role]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Role] OFF;
    END

DROP TABLE [dbo].[Role];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Role]', N'Role';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Role]', N'PK_Role', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[SBCValue]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_SBCValue] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          INT            NULL,
    [FieldPositionID] INT            NULL,
    [TeamID]          INT            NULL,
    [AddedDate]       DATETIME       NOT NULL,
    [Squat]           FLOAT (53)     NOT NULL,
    [Bench]           FLOAT (53)     NOT NULL,
    [Clean]           FLOAT (53)     NOT NULL,
    [FirstName]       NVARCHAR (500) NULL,
    [LastName]        NVARCHAR (500) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_SBCValue] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[SBCValue])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SBCValue] ON;
        INSERT INTO [dbo].[tmp_ms_xx_SBCValue] ([ID], [UserID], [FieldPositionID], [TeamID], [AddedDate], [Squat], [Bench], [Clean], [FirstName], [LastName])
        SELECT   [ID],
                 [UserID],
                 [FieldPositionID],
                 [TeamID],
                 [AddedDate],
                 [Squat],
                 [Bench],
                 [Clean],
                 [FirstName],
                 [LastName]
        FROM     [dbo].[SBCValue]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SBCValue] OFF;
    END

DROP TABLE [dbo].[SBCValue];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_SBCValue]', N'SBCValue';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_SBCValue]', N'PK_SBCValue', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Schedule]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Schedule] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [UserSeasonID] INT NULL,
    [TeamID]       INT NULL,
    [GroupID]      INT NULL,
    [Number]       INT NOT NULL,
    [MacrocycleID] INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Schedule] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Schedule])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Schedule] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Schedule] ([ID], [UserSeasonID], [TeamID], [GroupID], [Number], [MacrocycleID])
        SELECT   [ID],
                 [UserSeasonID],
                 [TeamID],
                 [GroupID],
                 [Number],
                 [MacrocycleID]
        FROM     [dbo].[Schedule]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Schedule] OFF;
    END

DROP TABLE [dbo].[Schedule];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Schedule]', N'Schedule';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Schedule]', N'PK_Schedule', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Season]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Season] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Type] INT            NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Season] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Season])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Season] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Season] ([ID], [Type], [Name])
        SELECT   [ID],
                 [Type],
                 [Name]
        FROM     [dbo].[Season]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Season] OFF;
    END

DROP TABLE [dbo].[Season];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Season]', N'Season';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Season]', N'PK_Season', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Setting]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Setting] (
    [ID]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (50)  NOT NULL,
    [Value] NVARCHAR (500) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Setting] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Setting])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Setting] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Setting] ([ID], [Name], [Value])
        SELECT   [ID],
                 [Name],
                 [Value]
        FROM     [dbo].[Setting]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Setting] OFF;
    END

DROP TABLE [dbo].[Setting];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Setting]', N'Setting';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Setting]', N'PK_Setting', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[State]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_State] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    [Code] NVARCHAR (50)  NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_State] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[State])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_State] ON;
        INSERT INTO [dbo].[tmp_ms_xx_State] ([ID], [Name], [Code])
        SELECT   [ID],
                 [Name],
                 [Code]
        FROM     [dbo].[State]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_State] OFF;
    END

DROP TABLE [dbo].[State];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_State]', N'State';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_State]', N'PK_State', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Team]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Team] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [UserID]         INT            NOT NULL,
    [Name]           NVARCHAR (500) NOT NULL,
    [LogoPath]       NVARCHAR (150) NULL,
    [StateID]        INT            NOT NULL,
    [PrimaryColor]   NVARCHAR (10)  NOT NULL,
    [SecondaryColor] NVARCHAR (10)  NOT NULL,
    [SBCControl]     INT            NOT NULL,
    [MaxCount]       INT            DEFAULT ((100)) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Team] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Team])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Team] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Team] ([ID], [UserID], [Name], [LogoPath], [StateID], [PrimaryColor], [SecondaryColor], [SBCControl], [MaxCount])
        SELECT   [ID],
                 [UserID],
                 [Name],
                 [LogoPath],
                 [StateID],
                 [PrimaryColor],
                 [SecondaryColor],
                 [SBCControl],
                 [MaxCount]
        FROM     [dbo].[Team]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Team] OFF;
    END

DROP TABLE [dbo].[Team];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Team]', N'Team';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Team]', N'PK_Team', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Training]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Training] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Training] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Training])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Training] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Training] ([ID], [Name])
        SELECT   [ID],
                 [Name]
        FROM     [dbo].[Training]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Training] OFF;
    END

DROP TABLE [dbo].[Training];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Training]', N'Training';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Training]', N'PK_Training', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[TrainingDay]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_TrainingDay] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [WeekID]       INT NOT NULL,
    [MacrocycleID] INT NULL,
    [DayID]        INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_TrainingDay] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[TrainingDay])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_TrainingDay] ON;
        INSERT INTO [dbo].[tmp_ms_xx_TrainingDay] ([ID], [WeekID], [MacrocycleID], [DayID])
        SELECT   [ID],
                 [WeekID],
                 [MacrocycleID],
                 [DayID]
        FROM     [dbo].[TrainingDay]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_TrainingDay] OFF;
    END

DROP TABLE [dbo].[TrainingDay];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_TrainingDay]', N'TrainingDay';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_TrainingDay]', N'PK_TrainingDay', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[TrainingDayCell]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_TrainingDayCell] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [CellID]        INT           NOT NULL,
    [TrainingDayID] INT           NOT NULL,
    [PrimaryText]   NVARCHAR (50) NULL,
    [TrainingSetID] INT           NULL,
    [SBCType]       INT           NULL,
    [Coefficient]   FLOAT (53)    NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_TrainingDayCell] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[TrainingDayCell])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_TrainingDayCell] ON;
        INSERT INTO [dbo].[tmp_ms_xx_TrainingDayCell] ([ID], [CellID], [TrainingDayID], [PrimaryText], [TrainingSetID], [SBCType], [Coefficient])
        SELECT   [ID],
                 [CellID],
                 [TrainingDayID],
                 [PrimaryText],
                 [TrainingSetID],
                 [SBCType],
                 [Coefficient]
        FROM     [dbo].[TrainingDayCell]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_TrainingDayCell] OFF;
    END

DROP TABLE [dbo].[TrainingDayCell];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_TrainingDayCell]', N'TrainingDayCell';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_TrainingDayCell]', N'PK_TrainingDayCell', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[TrainingEquipment]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_TrainingEquipment] (
    [ID]            INT IDENTITY (1, 1) NOT NULL,
    [TrainingSetID] INT NOT NULL,
    [TrainingID]    INT NOT NULL,
    [EquipmentID]   INT NULL,
    [Equipment2ID]  INT NULL,
    [Priority]      INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_TrainingEquipment] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[TrainingEquipment])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_TrainingEquipment] ON;
        INSERT INTO [dbo].[tmp_ms_xx_TrainingEquipment] ([ID], [TrainingSetID], [TrainingID], [EquipmentID], [Equipment2ID], [Priority])
        SELECT   [ID],
                 [TrainingSetID],
                 [TrainingID],
                 [EquipmentID],
                 [Equipment2ID],
                 [Priority]
        FROM     [dbo].[TrainingEquipment]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_TrainingEquipment] OFF;
    END

DROP TABLE [dbo].[TrainingEquipment];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_TrainingEquipment]', N'TrainingEquipment';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_TrainingEquipment]', N'PK_TrainingEquipment', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[TrainingSet]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_TrainingSet] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [DayID]   INT NOT NULL,
    [PhaseID] INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_TrainingSet] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[TrainingSet])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_TrainingSet] ON;
        INSERT INTO [dbo].[tmp_ms_xx_TrainingSet] ([ID], [DayID], [PhaseID])
        SELECT   [ID],
                 [DayID],
                 [PhaseID]
        FROM     [dbo].[TrainingSet]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_TrainingSet] OFF;
    END

DROP TABLE [dbo].[TrainingSet];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_TrainingSet]', N'TrainingSet';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_TrainingSet]', N'PK_TrainingSet', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[User]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_User] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
    [Email]                   NVARCHAR (150) NOT NULL,
    [Password]                NVARCHAR (50)  NOT NULL,
    [AddedDate]               DATETIME       NOT NULL,
    [ActivatedDate]           DATETIME       NULL,
    [ActivatedLink]           NVARCHAR (50)  NOT NULL,
    [LastVisitDate]           DATETIME       NOT NULL,
    [AvatarPath]              NVARCHAR (150) NULL,
    [FirstName]               NVARCHAR (500) NULL,
    [LastName]                NVARCHAR (500) NULL,
    [PaidTill]                DATETIME       NULL,
    [PhoneNumber]             NVARCHAR (50)  NULL,
    [PlayerOfTeamID]          INT            NULL,
    [GroupID]                 INT            NULL,
    [FieldPositionID]         INT            NULL,
    [VisitGettingStartedPage] BIT            NULL,
    [Year]                    INT            NULL,
    [Squat]                   FLOAT (53)     NOT NULL,
    [Bench]                   FLOAT (53)     NOT NULL,
    [Clean]                   FLOAT (53)     NOT NULL,
    [Height]                  NVARCHAR (50)  NULL,
    [Weight]                  NVARCHAR (50)  NULL,
    [BodyFat]                 NVARCHAR (50)  NULL,
    [_40YardDash]             FLOAT (53)     NULL,
    [Vertical]                FLOAT (53)     NULL,
    [_3Cone]                  FLOAT (53)     NULL,
    [TDrill]                  FLOAT (53)     NULL,
    [PrimaryColor]            NVARCHAR (50)  NULL,
    [SecondaryColor]          NVARCHAR (50)  NULL,
    [LoginInfoSent]           DATETIME       NULL,
    [AttendanceStartDate]     DATETIME       NULL,
    [ProgressStartDate]       DATETIME       NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_User] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[User])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_User] ON;
        INSERT INTO [dbo].[tmp_ms_xx_User] ([ID], [Email], [Password], [AddedDate], [ActivatedDate], [ActivatedLink], [LastVisitDate], [AvatarPath], [FirstName], [LastName], [PaidTill], [PhoneNumber], [PlayerOfTeamID], [GroupID], [FieldPositionID], [VisitGettingStartedPage], [Year], [Squat], [Bench], [Clean], [Height], [Weight], [BodyFat], [_40YardDash], [Vertical], [_3Cone], [TDrill], [PrimaryColor], [SecondaryColor], [LoginInfoSent], [AttendanceStartDate], [ProgressStartDate])
        SELECT   [ID],
                 [Email],
                 [Password],
                 [AddedDate],
                 [ActivatedDate],
                 [ActivatedLink],
                 [LastVisitDate],
                 [AvatarPath],
                 [FirstName],
                 [LastName],
                 [PaidTill],
                 [PhoneNumber],
                 [PlayerOfTeamID],
                 [GroupID],
                 [FieldPositionID],
                 [VisitGettingStartedPage],
                 [Year],
                 [Squat],
                 [Bench],
                 [Clean],
                 [Height],
                 [Weight],
                 [BodyFat],
                 [_40YardDash],
                 [Vertical],
                 [_3Cone],
                 [TDrill],
                 [PrimaryColor],
                 [SecondaryColor],
                 [LoginInfoSent],
                 [AttendanceStartDate],
                 [ProgressStartDate]
        FROM     [dbo].[User]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_User] OFF;
    END

DROP TABLE [dbo].[User];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_User]', N'User';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_User]', N'PK_User', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[UserAttendance]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_UserAttendance] (
    [ID]           INT      IDENTITY (1, 1) NOT NULL,
    [UserSeasonID] INT      NOT NULL,
    [UserID]       INT      NOT NULL,
    [AddedDate]    DATETIME NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_UserAttendance] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[UserAttendance])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserAttendance] ON;
        INSERT INTO [dbo].[tmp_ms_xx_UserAttendance] ([ID], [UserSeasonID], [UserID], [AddedDate])
        SELECT   [ID],
                 [UserSeasonID],
                 [UserID],
                 [AddedDate]
        FROM     [dbo].[UserAttendance]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserAttendance] OFF;
    END

DROP TABLE [dbo].[UserAttendance];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_UserAttendance]', N'UserAttendance';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_UserAttendance]', N'PK_UserAttendance', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[UserEquipment]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_UserEquipment] (
    [ID]          INT IDENTITY (1, 1) NOT NULL,
    [UserID]      INT NOT NULL,
    [EquipmentID] INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_UserEquipment] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[UserEquipment])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserEquipment] ON;
        INSERT INTO [dbo].[tmp_ms_xx_UserEquipment] ([ID], [UserID], [EquipmentID])
        SELECT   [ID],
                 [UserID],
                 [EquipmentID]
        FROM     [dbo].[UserEquipment]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserEquipment] OFF;
    END

DROP TABLE [dbo].[UserEquipment];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_UserEquipment]', N'UserEquipment';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_UserEquipment]', N'PK_UserEquipment', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[UserPillar]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_UserPillar] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [UserID]       INT           NOT NULL,
    [PillarTypeID] INT           NOT NULL,
    [Value]        INT           NOT NULL,
    [AddedDate]    DATETIME      NULL,
    [TextValue]    NVARCHAR (50) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_UserPillar] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[UserPillar])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserPillar] ON;
        INSERT INTO [dbo].[tmp_ms_xx_UserPillar] ([ID], [UserID], [PillarTypeID], [Value], [AddedDate], [TextValue])
        SELECT   [ID],
                 [UserID],
                 [PillarTypeID],
                 [Value],
                 [AddedDate],
                 [TextValue]
        FROM     [dbo].[UserPillar]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserPillar] OFF;
    END

DROP TABLE [dbo].[UserPillar];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_UserPillar]', N'UserPillar';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_UserPillar]', N'PK_UserPillar', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[UserRole]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_UserRole] (
    [ID]     INT IDENTITY (1, 1) NOT NULL,
    [RoleID] INT NOT NULL,
    [UserID] INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_UserRole] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[UserRole])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserRole] ON;
        INSERT INTO [dbo].[tmp_ms_xx_UserRole] ([ID], [RoleID], [UserID])
        SELECT   [ID],
                 [RoleID],
                 [UserID]
        FROM     [dbo].[UserRole]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserRole] OFF;
    END

DROP TABLE [dbo].[UserRole];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_UserRole]', N'UserRole';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_UserRole]', N'PK_UserRole', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[UserSeason]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_UserSeason] (
    [ID]       INT      IDENTITY (1, 1) NOT NULL,
    [SeasonID] INT      NOT NULL,
    [UserID]   INT      NOT NULL,
    [StartDay] DATETIME NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_UserSeason] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[UserSeason])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserSeason] ON;
        INSERT INTO [dbo].[tmp_ms_xx_UserSeason] ([ID], [SeasonID], [UserID], [StartDay])
        SELECT   [ID],
                 [SeasonID],
                 [UserID],
                 [StartDay]
        FROM     [dbo].[UserSeason]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserSeason] OFF;
    END

DROP TABLE [dbo].[UserSeason];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_UserSeason]', N'UserSeason';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_UserSeason]', N'PK_UserSeason', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Video]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Video] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [TrainingID] INT            NULL,
    [Header]     NVARCHAR (500) NOT NULL,
    [Text]       NVARCHAR (MAX) NULL,
    [VideoUrl]   NVARCHAR (500) NOT NULL,
    [VideoCode]  NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Video] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Video])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Video] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Video] ([ID], [TrainingID], [Header], [Text], [VideoUrl], [VideoCode])
        SELECT   [ID],
                 [TrainingID],
                 [Header],
                 [Text],
                 [VideoUrl],
                 [VideoCode]
        FROM     [dbo].[Video]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Video] OFF;
    END

DROP TABLE [dbo].[Video];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Video]', N'Video';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Video]', N'PK_Video', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Week]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Week] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [PhaseID] INT            NOT NULL,
    [Number]  INT            NULL,
    [Name]    NVARCHAR (500) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Week] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Week])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Week] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Week] ([ID], [PhaseID], [Number], [Name])
        SELECT   [ID],
                 [PhaseID],
                 [Number],
                 [Name]
        FROM     [dbo].[Week]
        ORDER BY [ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Week] OFF;
    END

DROP TABLE [dbo].[Week];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Week]', N'Week';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Week]', N'PK_Week', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[FK_Banner_BannerPlace]...';


GO
ALTER TABLE [dbo].[Banner] WITH NOCHECK
    ADD CONSTRAINT [FK_Banner_BannerPlace] FOREIGN KEY ([BannerPlaceID]) REFERENCES [dbo].[BannerPlace] ([ID]);


GO
PRINT N'Creating [dbo].[FK_BillingInfo_State]...';


GO
ALTER TABLE [dbo].[BillingInfo] WITH NOCHECK
    ADD CONSTRAINT [FK_BillingInfo_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID]);


GO
PRINT N'Creating [dbo].[FK_BillingInfo_User]...';


GO
ALTER TABLE [dbo].[BillingInfo] WITH NOCHECK
    ADD CONSTRAINT [FK_BillingInfo_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Cycle_Season]...';


GO
ALTER TABLE [dbo].[Cycle] WITH NOCHECK
    ADD CONSTRAINT [FK_Cycle_Season] FOREIGN KEY ([SeasonID]) REFERENCES [dbo].[Season] ([ID]);


GO
PRINT N'Creating [dbo].[FK_FeatureText_FeatureCatalog]...';


GO
ALTER TABLE [dbo].[FeatureText] WITH NOCHECK
    ADD CONSTRAINT [FK_FeatureText_FeatureCatalog] FOREIGN KEY ([FeatureCatalogID]) REFERENCES [dbo].[FeatureCatalog] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Feedback_State]...';


GO
ALTER TABLE [dbo].[Feedback] WITH NOCHECK
    ADD CONSTRAINT [FK_Feedback_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Group_Team]...';


GO
ALTER TABLE [dbo].[Group] WITH NOCHECK
    ADD CONSTRAINT [FK_Group_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Invoice_State]...';


GO
ALTER TABLE [dbo].[Invoice] WITH NOCHECK
    ADD CONSTRAINT [FK_Invoice_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Invoice_User]...';


GO
ALTER TABLE [dbo].[Invoice] WITH NOCHECK
    ADD CONSTRAINT [FK_Invoice_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Macrocycle_Week]...';


GO
ALTER TABLE [dbo].[Macrocycle] WITH NOCHECK
    ADD CONSTRAINT [FK_Macrocycle_Week] FOREIGN KEY ([WeekID]) REFERENCES [dbo].[Week] ([ID]);


GO
PRINT N'Creating [dbo].[FK_PaymentDetail_User]...';


GO
ALTER TABLE [dbo].[PaymentDetail] WITH NOCHECK
    ADD CONSTRAINT [FK_PaymentDetail_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PersonalSchedule_Macrocycle]...';


GO
ALTER TABLE [dbo].[PersonalSchedule] WITH NOCHECK
    ADD CONSTRAINT [FK_PersonalSchedule_Macrocycle] FOREIGN KEY ([MacrocycleID]) REFERENCES [dbo].[Macrocycle] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PersonalSchedule_User]...';


GO
ALTER TABLE [dbo].[PersonalSchedule] WITH NOCHECK
    ADD CONSTRAINT [FK_PersonalSchedule_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PersonalSchedule_UserSeason]...';


GO
ALTER TABLE [dbo].[PersonalSchedule] WITH NOCHECK
    ADD CONSTRAINT [FK_PersonalSchedule_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Phase_Cycle]...';


GO
ALTER TABLE [dbo].[Phase] WITH NOCHECK
    ADD CONSTRAINT [FK_Phase_Cycle] FOREIGN KEY ([CycleID]) REFERENCES [dbo].[Cycle] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Post_User]...';


GO
ALTER TABLE [dbo].[Post] WITH NOCHECK
    ADD CONSTRAINT [FK_Post_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PromoCode_PromoAction]...';


GO
ALTER TABLE [dbo].[PromoCode] WITH NOCHECK
    ADD CONSTRAINT [FK_PromoCode_PromoAction] FOREIGN KEY ([PromoActionID]) REFERENCES [dbo].[PromoAction] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_SBCValue_FieldPosition]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_FieldPosition] FOREIGN KEY ([FieldPositionID]) REFERENCES [dbo].[FieldPosition] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_SBCValue_Team]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]);


GO
PRINT N'Creating [dbo].[FK_SBCValue_User]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Schedule_Group]...';


GO
ALTER TABLE [dbo].[Schedule] WITH NOCHECK
    ADD CONSTRAINT [FK_Schedule_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Schedule_Macrocycle]...';


GO
ALTER TABLE [dbo].[Schedule] WITH NOCHECK
    ADD CONSTRAINT [FK_Schedule_Macrocycle] FOREIGN KEY ([MacrocycleID]) REFERENCES [dbo].[Macrocycle] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Schedule_Team]...';


GO
ALTER TABLE [dbo].[Schedule] WITH NOCHECK
    ADD CONSTRAINT [FK_Schedule_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Schedule_UserSeason]...';


GO
ALTER TABLE [dbo].[Schedule] WITH NOCHECK
    ADD CONSTRAINT [FK_Schedule_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Team_State]...';


GO
ALTER TABLE [dbo].[Team] WITH NOCHECK
    ADD CONSTRAINT [FK_Team_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Team_User]...';


GO
ALTER TABLE [dbo].[Team] WITH NOCHECK
    ADD CONSTRAINT [FK_Team_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TrainingDay_Week]...';


GO
ALTER TABLE [dbo].[TrainingDay] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingDay_Week] FOREIGN KEY ([WeekID]) REFERENCES [dbo].[Week] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TrainingDay_Day]...';


GO
ALTER TABLE [dbo].[TrainingDay] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingDay_Day] FOREIGN KEY ([DayID]) REFERENCES [dbo].[Day] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TrainingDay_Macrocycle]...';


GO
ALTER TABLE [dbo].[TrainingDay] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingDay_Macrocycle] FOREIGN KEY ([MacrocycleID]) REFERENCES [dbo].[Macrocycle] ([ID]) ON DELETE SET NULL ON UPDATE SET NULL;


GO
PRINT N'Creating [dbo].[FK_TrainingDayCell_Cell]...';


GO
ALTER TABLE [dbo].[TrainingDayCell] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingDayCell_Cell] FOREIGN KEY ([CellID]) REFERENCES [dbo].[Cell] ([ID]);


GO
PRINT N'Creating [dbo].[FK_TrainingDayCell_TrainingDay]...';


GO
ALTER TABLE [dbo].[TrainingDayCell] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingDayCell_TrainingDay] FOREIGN KEY ([TrainingDayID]) REFERENCES [dbo].[TrainingDay] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TrainingDayCell_TrainingSet]...';


GO
ALTER TABLE [dbo].[TrainingDayCell] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingDayCell_TrainingSet] FOREIGN KEY ([TrainingSetID]) REFERENCES [dbo].[TrainingSet] ([ID]);


GO
PRINT N'Creating [dbo].[FK_TrainingEquipment_Equipment]...';


GO
ALTER TABLE [dbo].[TrainingEquipment] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingEquipment_Equipment] FOREIGN KEY ([EquipmentID]) REFERENCES [dbo].[Equipment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TrainingEquipment_Equipment2]...';


GO
ALTER TABLE [dbo].[TrainingEquipment] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingEquipment_Equipment2] FOREIGN KEY ([Equipment2ID]) REFERENCES [dbo].[Equipment] ([ID]);


GO
PRINT N'Creating [dbo].[FK_TrainingEquipment_Training]...';


GO
ALTER TABLE [dbo].[TrainingEquipment] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingEquipment_Training] FOREIGN KEY ([TrainingID]) REFERENCES [dbo].[Training] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TrainingEquipment_TrainingSet]...';


GO
ALTER TABLE [dbo].[TrainingEquipment] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingEquipment_TrainingSet] FOREIGN KEY ([TrainingSetID]) REFERENCES [dbo].[TrainingSet] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TrainingSet_Day]...';


GO
ALTER TABLE [dbo].[TrainingSet] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingSet_Day] FOREIGN KEY ([DayID]) REFERENCES [dbo].[Day] ([ID]);


GO
PRINT N'Creating [dbo].[FK_TrainingSet_Phase]...';


GO
ALTER TABLE [dbo].[TrainingSet] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingSet_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID]);


GO
PRINT N'Creating [dbo].[FK_User_FieldPosition]...';


GO
ALTER TABLE [dbo].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_FieldPosition] FOREIGN KEY ([FieldPositionID]) REFERENCES [dbo].[FieldPosition] ([ID]);


GO
PRINT N'Creating [dbo].[FK_User_Group]...';


GO
ALTER TABLE [dbo].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]);


GO
PRINT N'Creating [dbo].[FK_User_Team]...';


GO
ALTER TABLE [dbo].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Team] FOREIGN KEY ([PlayerOfTeamID]) REFERENCES [dbo].[Team] ([ID]);


GO
PRINT N'Creating [dbo].[FK_UserAttendance_User]...';


GO
ALTER TABLE [dbo].[UserAttendance] WITH NOCHECK
    ADD CONSTRAINT [FK_UserAttendance_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserAttendance_UserSeason]...';


GO
ALTER TABLE [dbo].[UserAttendance] WITH NOCHECK
    ADD CONSTRAINT [FK_UserAttendance_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID]);


GO
PRINT N'Creating [dbo].[FK_UserEquipment_Equipment]...';


GO
ALTER TABLE [dbo].[UserEquipment] WITH NOCHECK
    ADD CONSTRAINT [FK_UserEquipment_Equipment] FOREIGN KEY ([EquipmentID]) REFERENCES [dbo].[Equipment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserEquipment_User]...';


GO
ALTER TABLE [dbo].[UserEquipment] WITH NOCHECK
    ADD CONSTRAINT [FK_UserEquipment_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserPillar_PillarType]...';


GO
ALTER TABLE [dbo].[UserPillar] WITH NOCHECK
    ADD CONSTRAINT [FK_UserPillar_PillarType] FOREIGN KEY ([PillarTypeID]) REFERENCES [dbo].[PillarType] ([ID]);


GO
PRINT N'Creating [dbo].[FK_UserPillar_User]...';


GO
ALTER TABLE [dbo].[UserPillar] WITH NOCHECK
    ADD CONSTRAINT [FK_UserPillar_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserRole_Role]...';


GO
ALTER TABLE [dbo].[UserRole] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserRole_User]...';


GO
ALTER TABLE [dbo].[UserRole] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserSeason_Season]...';


GO
ALTER TABLE [dbo].[UserSeason] WITH NOCHECK
    ADD CONSTRAINT [FK_UserSeason_Season] FOREIGN KEY ([SeasonID]) REFERENCES [dbo].[Season] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserSeason_User]...';


GO
ALTER TABLE [dbo].[UserSeason] WITH NOCHECK
    ADD CONSTRAINT [FK_UserSeason_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Video_Training]...';


GO
ALTER TABLE [dbo].[Video] WITH NOCHECK
    ADD CONSTRAINT [FK_Video_Training] FOREIGN KEY ([TrainingID]) REFERENCES [dbo].[Training] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Week_Phase]...';


GO
ALTER TABLE [dbo].[Week] WITH NOCHECK
    ADD CONSTRAINT [FK_Week_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Checking existing data against newly created constraints';




GO
ALTER TABLE [dbo].[Banner] WITH CHECK CHECK CONSTRAINT [FK_Banner_BannerPlace];

ALTER TABLE [dbo].[BillingInfo] WITH CHECK CHECK CONSTRAINT [FK_BillingInfo_State];

ALTER TABLE [dbo].[BillingInfo] WITH CHECK CHECK CONSTRAINT [FK_BillingInfo_User];

ALTER TABLE [dbo].[Cycle] WITH CHECK CHECK CONSTRAINT [FK_Cycle_Season];

ALTER TABLE [dbo].[FeatureText] WITH CHECK CHECK CONSTRAINT [FK_FeatureText_FeatureCatalog];

ALTER TABLE [dbo].[Feedback] WITH CHECK CHECK CONSTRAINT [FK_Feedback_State];

ALTER TABLE [dbo].[Group] WITH CHECK CHECK CONSTRAINT [FK_Group_Team];

ALTER TABLE [dbo].[Invoice] WITH CHECK CHECK CONSTRAINT [FK_Invoice_State];

ALTER TABLE [dbo].[Invoice] WITH CHECK CHECK CONSTRAINT [FK_Invoice_User];

ALTER TABLE [dbo].[Macrocycle] WITH CHECK CHECK CONSTRAINT [FK_Macrocycle_Week];

ALTER TABLE [dbo].[PaymentDetail] WITH CHECK CHECK CONSTRAINT [FK_PaymentDetail_User];

ALTER TABLE [dbo].[PersonalSchedule] WITH CHECK CHECK CONSTRAINT [FK_PersonalSchedule_Macrocycle];

ALTER TABLE [dbo].[PersonalSchedule] WITH CHECK CHECK CONSTRAINT [FK_PersonalSchedule_User];

ALTER TABLE [dbo].[PersonalSchedule] WITH CHECK CHECK CONSTRAINT [FK_PersonalSchedule_UserSeason];

ALTER TABLE [dbo].[Phase] WITH CHECK CHECK CONSTRAINT [FK_Phase_Cycle];

ALTER TABLE [dbo].[Post] WITH CHECK CHECK CONSTRAINT [FK_Post_User];

ALTER TABLE [dbo].[PromoCode] WITH CHECK CHECK CONSTRAINT [FK_PromoCode_PromoAction];

ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_FieldPosition];

ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_Team];

ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_User];

ALTER TABLE [dbo].[Schedule] WITH CHECK CHECK CONSTRAINT [FK_Schedule_Group];

ALTER TABLE [dbo].[Schedule] WITH CHECK CHECK CONSTRAINT [FK_Schedule_Macrocycle];

ALTER TABLE [dbo].[Schedule] WITH CHECK CHECK CONSTRAINT [FK_Schedule_Team];

ALTER TABLE [dbo].[Schedule] WITH CHECK CHECK CONSTRAINT [FK_Schedule_UserSeason];

ALTER TABLE [dbo].[Team] WITH CHECK CHECK CONSTRAINT [FK_Team_State];

ALTER TABLE [dbo].[Team] WITH CHECK CHECK CONSTRAINT [FK_Team_User];

ALTER TABLE [dbo].[TrainingDay] WITH CHECK CHECK CONSTRAINT [FK_TrainingDay_Week];

ALTER TABLE [dbo].[TrainingDay] WITH CHECK CHECK CONSTRAINT [FK_TrainingDay_Day];

ALTER TABLE [dbo].[TrainingDay] WITH CHECK CHECK CONSTRAINT [FK_TrainingDay_Macrocycle];

ALTER TABLE [dbo].[TrainingDayCell] WITH CHECK CHECK CONSTRAINT [FK_TrainingDayCell_Cell];

ALTER TABLE [dbo].[TrainingDayCell] WITH CHECK CHECK CONSTRAINT [FK_TrainingDayCell_TrainingDay];

ALTER TABLE [dbo].[TrainingDayCell] WITH CHECK CHECK CONSTRAINT [FK_TrainingDayCell_TrainingSet];

ALTER TABLE [dbo].[TrainingEquipment] WITH CHECK CHECK CONSTRAINT [FK_TrainingEquipment_Equipment];

ALTER TABLE [dbo].[TrainingEquipment] WITH CHECK CHECK CONSTRAINT [FK_TrainingEquipment_Equipment2];

ALTER TABLE [dbo].[TrainingEquipment] WITH CHECK CHECK CONSTRAINT [FK_TrainingEquipment_Training];

ALTER TABLE [dbo].[TrainingEquipment] WITH CHECK CHECK CONSTRAINT [FK_TrainingEquipment_TrainingSet];

ALTER TABLE [dbo].[TrainingSet] WITH CHECK CHECK CONSTRAINT [FK_TrainingSet_Day];

ALTER TABLE [dbo].[TrainingSet] WITH CHECK CHECK CONSTRAINT [FK_TrainingSet_Phase];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_FieldPosition];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Group];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Team];

ALTER TABLE [dbo].[UserAttendance] WITH CHECK CHECK CONSTRAINT [FK_UserAttendance_User];

ALTER TABLE [dbo].[UserAttendance] WITH CHECK CHECK CONSTRAINT [FK_UserAttendance_UserSeason];

ALTER TABLE [dbo].[UserEquipment] WITH CHECK CHECK CONSTRAINT [FK_UserEquipment_Equipment];

ALTER TABLE [dbo].[UserEquipment] WITH CHECK CHECK CONSTRAINT [FK_UserEquipment_User];

ALTER TABLE [dbo].[UserPillar] WITH CHECK CHECK CONSTRAINT [FK_UserPillar_PillarType];

ALTER TABLE [dbo].[UserPillar] WITH CHECK CHECK CONSTRAINT [FK_UserPillar_User];

ALTER TABLE [dbo].[UserRole] WITH CHECK CHECK CONSTRAINT [FK_UserRole_Role];

ALTER TABLE [dbo].[UserRole] WITH CHECK CHECK CONSTRAINT [FK_UserRole_User];

ALTER TABLE [dbo].[UserSeason] WITH CHECK CHECK CONSTRAINT [FK_UserSeason_Season];

ALTER TABLE [dbo].[UserSeason] WITH CHECK CHECK CONSTRAINT [FK_UserSeason_User];

ALTER TABLE [dbo].[Video] WITH CHECK CHECK CONSTRAINT [FK_Video_Training];

ALTER TABLE [dbo].[Week] WITH CHECK CHECK CONSTRAINT [FK_Week_Phase];


GO
PRINT N'Update complete.';


GO
