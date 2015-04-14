
GO
PRINT N'Dropping [dbo].[FK_PersonalSchedule_UserSeason]...';


GO
ALTER TABLE [dbo].[PersonalSchedule] DROP CONSTRAINT [FK_PersonalSchedule_UserSeason];


GO
PRINT N'Dropping [dbo].[FK_Schedule_UserSeason]...';


GO
ALTER TABLE [dbo].[Schedule] DROP CONSTRAINT [FK_Schedule_UserSeason];


GO
PRINT N'Dropping [dbo].[FK_UserAttendance_UserSeason]...';


GO
ALTER TABLE [dbo].[UserAttendance] DROP CONSTRAINT [FK_UserAttendance_UserSeason];


GO
PRINT N'Dropping [dbo].[FK_UserSeason_User]...';


GO
ALTER TABLE [dbo].[UserSeason] DROP CONSTRAINT [FK_UserSeason_User];


GO
PRINT N'Dropping [dbo].[FK_UserSeason_Season]...';


GO
ALTER TABLE [dbo].[UserSeason] DROP CONSTRAINT [FK_UserSeason_Season];


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
    [GroupID]  INT      NULL,
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
PRINT N'Creating [dbo].[FK_PersonalSchedule_UserSeason]...';


GO
ALTER TABLE [dbo].[PersonalSchedule] WITH NOCHECK
    ADD CONSTRAINT [FK_PersonalSchedule_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Schedule_UserSeason]...';


GO
ALTER TABLE [dbo].[Schedule] WITH NOCHECK
    ADD CONSTRAINT [FK_Schedule_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID]);


GO
PRINT N'Creating [dbo].[FK_UserAttendance_UserSeason]...';


GO
ALTER TABLE [dbo].[UserAttendance] WITH NOCHECK
    ADD CONSTRAINT [FK_UserAttendance_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID]);


GO
PRINT N'Creating [dbo].[FK_UserSeason_User]...';


GO
ALTER TABLE [dbo].[UserSeason] WITH NOCHECK
    ADD CONSTRAINT [FK_UserSeason_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserSeason_Season]...';


GO
ALTER TABLE [dbo].[UserSeason] WITH NOCHECK
    ADD CONSTRAINT [FK_UserSeason_Season] FOREIGN KEY ([SeasonID]) REFERENCES [dbo].[Season] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserSeason_Group]...';


GO
ALTER TABLE [dbo].[UserSeason] WITH NOCHECK
    ADD CONSTRAINT [FK_UserSeason_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Checking existing data against newly created constraints';



GO
ALTER TABLE [dbo].[PersonalSchedule] WITH CHECK CHECK CONSTRAINT [FK_PersonalSchedule_UserSeason];

ALTER TABLE [dbo].[Schedule] WITH CHECK CHECK CONSTRAINT [FK_Schedule_UserSeason];

ALTER TABLE [dbo].[UserAttendance] WITH CHECK CHECK CONSTRAINT [FK_UserAttendance_UserSeason];

ALTER TABLE [dbo].[UserSeason] WITH CHECK CHECK CONSTRAINT [FK_UserSeason_User];

ALTER TABLE [dbo].[UserSeason] WITH CHECK CHECK CONSTRAINT [FK_UserSeason_Season];

ALTER TABLE [dbo].[UserSeason] WITH CHECK CHECK CONSTRAINT [FK_UserSeason_Group];


GO
PRINT N'Update complete.';


GO
