
GO
PRINT N'Dropping unnamed constraint on [dbo].[Team]...';


GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [DF__tmp_ms_xx__MaxCo__06CD04F7];


GO
PRINT N'Dropping [dbo].[FK_Team_State]...';


GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [FK_Team_State];


GO
PRINT N'Dropping [dbo].[FK_Team_User]...';


GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [FK_Team_User];


GO
PRINT N'Dropping [dbo].[FK_Group_Team]...';


GO
ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_Team];


GO
PRINT N'Dropping [dbo].[FK_SBCValue_Team]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_Team];


GO
PRINT N'Dropping [dbo].[FK_User_Team]...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Team];


GO
PRINT N'Dropping [dbo].[FK_Schedule_Team]...';


GO
ALTER TABLE [dbo].[Schedule] DROP CONSTRAINT [FK_Schedule_Team];


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
    [SchoolID]       INT            NULL,
    [PrimaryColor]   NVARCHAR (10)  NOT NULL,
    [SecondaryColor] NVARCHAR (10)  NOT NULL,
    [SBCControl]     INT            NOT NULL,
    [MaxCount]       INT            DEFAULT 100 NOT NULL,
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
PRINT N'Creating [dbo].[School]...';


GO
CREATE TABLE [dbo].[School] (
    [ID]      INT           IDENTITY (1, 1) NOT NULL,
    [StateID] INT           NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_School] PRIMARY KEY CLUSTERED ([ID] ASC)
);


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
PRINT N'Creating [dbo].[FK_Group_Team]...';


GO
ALTER TABLE [dbo].[Group] WITH NOCHECK
    ADD CONSTRAINT [FK_Group_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_SBCValue_Team]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]);


GO
PRINT N'Creating [dbo].[FK_User_Team]...';


GO
ALTER TABLE [dbo].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Team] FOREIGN KEY ([PlayerOfTeamID]) REFERENCES [dbo].[Team] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Schedule_Team]...';


GO
ALTER TABLE [dbo].[Schedule] WITH NOCHECK
    ADD CONSTRAINT [FK_Schedule_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Team_School]...';


GO
ALTER TABLE [dbo].[Team] WITH NOCHECK
    ADD CONSTRAINT [FK_Team_School] FOREIGN KEY ([SchoolID]) REFERENCES [dbo].[School] ([ID]);


GO
PRINT N'Creating [dbo].[FK_School_State]...';


GO
ALTER TABLE [dbo].[School] WITH NOCHECK
    ADD CONSTRAINT [FK_School_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID]);


GO
PRINT N'Checking existing data against newly created constraints';

GO
ALTER TABLE [dbo].[Team] WITH CHECK CHECK CONSTRAINT [FK_Team_State];

ALTER TABLE [dbo].[Team] WITH CHECK CHECK CONSTRAINT [FK_Team_User];

ALTER TABLE [dbo].[Group] WITH CHECK CHECK CONSTRAINT [FK_Group_Team];

ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_Team];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Team];

ALTER TABLE [dbo].[Schedule] WITH CHECK CHECK CONSTRAINT [FK_Schedule_Team];

ALTER TABLE [dbo].[Team] WITH CHECK CHECK CONSTRAINT [FK_Team_School];

ALTER TABLE [dbo].[School] WITH CHECK CHECK CONSTRAINT [FK_School_State];


GO
PRINT N'Update complete.';


GO
