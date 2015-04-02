
GO
PRINT N'Rename refactoring operation with key 0c021561-245b-4ae6-9401-9ecd2bd8a5fa is skipped, element [dbo].[UserFieldPosition].[Id] (SqlSimpleColumn) will not be renamed to ID';


GO
PRINT N'Dropping [dbo].[FK_User_FieldPosition]...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_FieldPosition];


GO
PRINT N'Dropping [dbo].[FK_SBCValue_FieldPosition]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_FieldPosition];


GO
PRINT N'Starting rebuilding table [dbo].[FieldPosition]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_FieldPosition] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [SportID] INT            NULL,
    [Code]    NVARCHAR (50)  NOT NULL,
    [Name]    NVARCHAR (500) NOT NULL,
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
PRINT N'Altering [dbo].[User]...';


GO
ALTER TABLE [dbo].[User]
    ADD [Birthday] DATETIME NULL,
        [Gender]   BIT      DEFAULT 1 NOT NULL;


GO
PRINT N'Creating [dbo].[Sport]...';


GO
CREATE TABLE [dbo].[Sport] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Sport] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[UserFieldPosition]...';


GO
CREATE TABLE [dbo].[UserFieldPosition] (
    [ID]              INT IDENTITY (1, 1) NOT NULL,
    [SportID]         INT NOT NULL,
    [UserID]          INT NOT NULL,
    [FieldPositionID] INT NULL,
    CONSTRAINT [PK_UserFieldPosition] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[FK_SBCValue_FieldPosition]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_FieldPosition] FOREIGN KEY ([FieldPositionID]) REFERENCES [dbo].[FieldPosition] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_FieldPosition_Sport]...';


GO
ALTER TABLE [dbo].[FieldPosition] WITH NOCHECK
    ADD CONSTRAINT [FK_FieldPosition_Sport] FOREIGN KEY ([SportID]) REFERENCES [dbo].[Sport] ([ID]);


GO
PRINT N'Creating [dbo].[FK_UserFieldPosition_Sport]...';


GO
ALTER TABLE [dbo].[UserFieldPosition] WITH NOCHECK
    ADD CONSTRAINT [FK_UserFieldPosition_Sport] FOREIGN KEY ([SportID]) REFERENCES [dbo].[Sport] ([ID]);


GO
PRINT N'Creating [dbo].[FK_UserFieldPosition_FieldPosition]...';


GO
ALTER TABLE [dbo].[UserFieldPosition] WITH NOCHECK
    ADD CONSTRAINT [FK_UserFieldPosition_FieldPosition] FOREIGN KEY ([FieldPositionID]) REFERENCES [dbo].[FieldPosition] ([ID]);


GO
-- Refactoring step to update target server with deployed transaction logs
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '0c021561-245b-4ae6-9401-9ecd2bd8a5fa')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('0c021561-245b-4ae6-9401-9ecd2bd8a5fa')

GO

GO
PRINT N'Checking existing data against newly created constraints';

GO
ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_FieldPosition];

ALTER TABLE [dbo].[FieldPosition] WITH CHECK CHECK CONSTRAINT [FK_FieldPosition_Sport];

ALTER TABLE [dbo].[UserFieldPosition] WITH CHECK CHECK CONSTRAINT [FK_UserFieldPosition_Sport];

ALTER TABLE [dbo].[UserFieldPosition] WITH CHECK CHECK CONSTRAINT [FK_UserFieldPosition_FieldPosition];


GO
PRINT N'Update complete.';


GO
