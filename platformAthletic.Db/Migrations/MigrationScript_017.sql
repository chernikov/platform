

GO
PRINT N'Dropping unnamed constraint on [dbo].[User]...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF__tmp_ms_xx__Gende__6BAEFA67];


GO
PRINT N'Dropping unnamed constraint on [dbo].[User]...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF__tmp_ms_xx__Publi__6CA31EA0];


GO
PRINT N'Dropping unnamed constraint on [dbo].[User]...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF__tmp_ms_xx__IsDel__6D9742D9];


GO
PRINT N'Dropping [dbo].[FK_UserAttendance_User]...';


GO
ALTER TABLE [dbo].[UserAttendance] DROP CONSTRAINT [FK_UserAttendance_User];


GO
PRINT N'Dropping [dbo].[FK_UserEquipment_User]...';


GO
ALTER TABLE [dbo].[UserEquipment] DROP CONSTRAINT [FK_UserEquipment_User];


GO
PRINT N'Dropping [dbo].[FK_UserPillar_User]...';


GO
ALTER TABLE [dbo].[UserPillar] DROP CONSTRAINT [FK_UserPillar_User];


GO
PRINT N'Dropping [dbo].[FK_UserRole_User]...';


GO
ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_UserRole_User];


GO
PRINT N'Dropping [dbo].[FK_UserSeason_User]...';


GO
ALTER TABLE [dbo].[UserSeason] DROP CONSTRAINT [FK_UserSeason_User];


GO
PRINT N'Dropping [dbo].[FK_UserVideo_User]...';


GO
ALTER TABLE [dbo].[UserVideo] DROP CONSTRAINT [FK_UserVideo_User];


GO
PRINT N'Dropping [dbo].[FK_BillingInfo_User]...';


GO
ALTER TABLE [dbo].[BillingInfo] DROP CONSTRAINT [FK_BillingInfo_User];


GO
PRINT N'Dropping [dbo].[FK_Invoice_User]...';


GO
ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_Invoice_User];


GO
PRINT N'Dropping [dbo].[FK_UserFieldPosition_User]...';


GO
ALTER TABLE [dbo].[UserFieldPosition] DROP CONSTRAINT [FK_UserFieldPosition_User];


GO
PRINT N'Dropping [dbo].[FK_Team_User]...';


GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [FK_Team_User];


GO
PRINT N'Dropping [dbo].[FK_User_Level]...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Level];


GO
PRINT N'Dropping [dbo].[FK_User_Group]...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Group];


GO
PRINT N'Dropping [dbo].[FK_User_Team]...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Team];


GO
PRINT N'Dropping [dbo].[FK_PaymentDetail_User]...';


GO
ALTER TABLE [dbo].[PaymentDetail] DROP CONSTRAINT [FK_PaymentDetail_User];


GO
PRINT N'Dropping [dbo].[FK_SBCValue_User]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_User];


GO
PRINT N'Dropping [dbo].[FK_PersonalSchedule_User]...';


GO
ALTER TABLE [dbo].[PersonalSchedule] DROP CONSTRAINT [FK_PersonalSchedule_User];


GO
PRINT N'Dropping [dbo].[FK_Post_User]...';


GO
ALTER TABLE [dbo].[Post] DROP CONSTRAINT [FK_Post_User];


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
    [AssistantOfTeamID]       INT            NULL,
    [GroupID]                 INT            NULL,
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
    [Birthday]                DATETIME       NULL,
    [Gender]                  BIT            DEFAULT 1 NOT NULL,
    [LevelID]                 INT            NULL,
    [GradYear]                INT            NULL,
    [PublicLevel]             INT            DEFAULT 2 NOT NULL,
    [IsDeleted]               BIT            DEFAULT 0 NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_User] PRIMARY KEY CLUSTERED ([ID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[User])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_User] ON;
        INSERT INTO [dbo].[tmp_ms_xx_User] ([ID], [Email], [Password], [AddedDate], [ActivatedDate], [ActivatedLink], [LastVisitDate], [AvatarPath], [FirstName], [LastName], [PaidTill], [PhoneNumber], [PlayerOfTeamID], [GroupID], [VisitGettingStartedPage], [Year], [Squat], [Bench], [Clean], [Height], [Weight], [BodyFat], [_40YardDash], [Vertical], [_3Cone], [TDrill], [PrimaryColor], [SecondaryColor], [LoginInfoSent], [AttendanceStartDate], [ProgressStartDate], [Birthday], [Gender], [LevelID], [GradYear], [PublicLevel], [IsDeleted])
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
                 [ProgressStartDate],
                 [Birthday],
                 [Gender],
                 [LevelID],
                 [GradYear],
                 [PublicLevel],
                 [IsDeleted]
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
PRINT N'Creating [dbo].[FK_UserAttendance_User]...';


GO
ALTER TABLE [dbo].[UserAttendance] WITH NOCHECK
    ADD CONSTRAINT [FK_UserAttendance_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserEquipment_User]...';


GO
ALTER TABLE [dbo].[UserEquipment] WITH NOCHECK
    ADD CONSTRAINT [FK_UserEquipment_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserPillar_User]...';


GO
ALTER TABLE [dbo].[UserPillar] WITH NOCHECK
    ADD CONSTRAINT [FK_UserPillar_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserRole_User]...';


GO
ALTER TABLE [dbo].[UserRole] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserSeason_User]...';


GO
ALTER TABLE [dbo].[UserSeason] WITH NOCHECK
    ADD CONSTRAINT [FK_UserSeason_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserVideo_User]...';


GO
ALTER TABLE [dbo].[UserVideo] WITH NOCHECK
    ADD CONSTRAINT [FK_UserVideo_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);


GO
PRINT N'Creating [dbo].[FK_BillingInfo_User]...';


GO
ALTER TABLE [dbo].[BillingInfo] WITH NOCHECK
    ADD CONSTRAINT [FK_BillingInfo_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Invoice_User]...';


GO
ALTER TABLE [dbo].[Invoice] WITH NOCHECK
    ADD CONSTRAINT [FK_Invoice_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserFieldPosition_User]...';


GO
ALTER TABLE [dbo].[UserFieldPosition] WITH NOCHECK
    ADD CONSTRAINT [FK_UserFieldPosition_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Team_User]...';


GO
ALTER TABLE [dbo].[Team] WITH NOCHECK
    ADD CONSTRAINT [FK_Team_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_User_Level]...';


GO
ALTER TABLE [dbo].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Level] FOREIGN KEY ([LevelID]) REFERENCES [dbo].[Level] ([ID]);


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
PRINT N'Creating [dbo].[FK_PaymentDetail_User]...';


GO
ALTER TABLE [dbo].[PaymentDetail] WITH NOCHECK
    ADD CONSTRAINT [FK_PaymentDetail_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_SBCValue_User]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PersonalSchedule_User]...';


GO
ALTER TABLE [dbo].[PersonalSchedule] WITH NOCHECK
    ADD CONSTRAINT [FK_PersonalSchedule_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Post_User]...';


GO
ALTER TABLE [dbo].[Post] WITH NOCHECK
    ADD CONSTRAINT [FK_Post_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_User_Team2]...';


GO
ALTER TABLE [dbo].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Team2] FOREIGN KEY ([AssistantOfTeamID]) REFERENCES [dbo].[Team] ([ID]);


GO
PRINT N'Checking existing data against newly created constraints';



GO
ALTER TABLE [dbo].[UserAttendance] WITH CHECK CHECK CONSTRAINT [FK_UserAttendance_User];

ALTER TABLE [dbo].[UserEquipment] WITH CHECK CHECK CONSTRAINT [FK_UserEquipment_User];

ALTER TABLE [dbo].[UserPillar] WITH CHECK CHECK CONSTRAINT [FK_UserPillar_User];

ALTER TABLE [dbo].[UserRole] WITH CHECK CHECK CONSTRAINT [FK_UserRole_User];

ALTER TABLE [dbo].[UserSeason] WITH CHECK CHECK CONSTRAINT [FK_UserSeason_User];

ALTER TABLE [dbo].[UserVideo] WITH CHECK CHECK CONSTRAINT [FK_UserVideo_User];

ALTER TABLE [dbo].[BillingInfo] WITH CHECK CHECK CONSTRAINT [FK_BillingInfo_User];

ALTER TABLE [dbo].[Invoice] WITH CHECK CHECK CONSTRAINT [FK_Invoice_User];

ALTER TABLE [dbo].[UserFieldPosition] WITH CHECK CHECK CONSTRAINT [FK_UserFieldPosition_User];

ALTER TABLE [dbo].[Team] WITH CHECK CHECK CONSTRAINT [FK_Team_User];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Level];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Group];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Team];

ALTER TABLE [dbo].[PaymentDetail] WITH CHECK CHECK CONSTRAINT [FK_PaymentDetail_User];

ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_User];

ALTER TABLE [dbo].[PersonalSchedule] WITH CHECK CHECK CONSTRAINT [FK_PersonalSchedule_User];

ALTER TABLE [dbo].[Post] WITH CHECK CHECK CONSTRAINT [FK_Post_User];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Team2];


GO
PRINT N'Update complete.';


GO
