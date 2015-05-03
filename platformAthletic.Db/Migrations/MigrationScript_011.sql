ALTER TABLE [dbo].[SBCValue] DROP COLUMN [SportID];



GO
ALTER TABLE [dbo].[Video]
    ADD [Preview] NVARCHAR (150) DEFAULT '' NOT NULL;
GO


GO
PRINT N'Dropping [dbo].[FK_UserPillar_PillarType]...';


GO
ALTER TABLE [dbo].[UserPillar] DROP CONSTRAINT [FK_UserPillar_PillarType];


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
    [Preview]     NVARCHAR (150) DEFAULT '' NULL,
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
PRINT N'Creating [dbo].[FK_UserPillar_PillarType]...';


GO
ALTER TABLE [dbo].[UserPillar] WITH NOCHECK
    ADD CONSTRAINT [FK_UserPillar_PillarType] FOREIGN KEY ([PillarTypeID]) REFERENCES [dbo].[PillarType] ([ID]);


GO
PRINT N'Checking existing data against newly created constraints';



GO
ALTER TABLE [dbo].[UserPillar] WITH CHECK CHECK CONSTRAINT [FK_UserPillar_PillarType];


GO
PRINT N'Update complete.';


GO
