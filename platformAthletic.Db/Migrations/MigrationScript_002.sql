
GO
PRINT N'Dropping [dbo].[FK_SBCValue_User]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_User];


GO
PRINT N'Dropping [dbo].[FK_SBCValue_FieldPosition]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_FieldPosition];


GO
PRINT N'Dropping [dbo].[FK_SBCValue_Team]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_Team];


GO
PRINT N'Starting rebuilding table [dbo].[SBCValue]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_SBCValue] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          INT            NULL,
    [SportID]         INT            NULL,
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
PRINT N'Altering [dbo].[User]...';


GO
ALTER TABLE [dbo].[User]
    ADD [IsDeleted] BIT DEFAULT 0 NOT NULL;


GO
PRINT N'Creating [dbo].[FK_SBCValue_User]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_SBCValue_FieldPosition]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_FieldPosition] FOREIGN KEY ([FieldPositionID]) REFERENCES [dbo].[FieldPosition] ([ID]);


GO
PRINT N'Creating [dbo].[FK_SBCValue_Team]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]);


GO
PRINT N'Creating [dbo].[FK_SBCValue_Sport]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_Sport] FOREIGN KEY ([SportID]) REFERENCES [dbo].[Sport] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_User];

ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_FieldPosition];

ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_Team];

ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_Sport];


GO
PRINT N'Update complete.';


GO
