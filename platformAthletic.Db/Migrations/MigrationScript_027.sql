
GO
ALTER TABLE [dbo].[UserAttendance] DROP CONSTRAINT [FK_UserAttendance_UserSeason];

GO
PRINT N'Altering [dbo].[UserAttendance]...';


GO
ALTER TABLE [dbo].[UserAttendance] ALTER COLUMN [UserSeasonID] INT NULL;


GO
PRINT N'Creating [dbo].[FK_UserAttendance_UserSeason]...';


GO
ALTER TABLE [dbo].[UserAttendance] WITH NOCHECK
    ADD CONSTRAINT [FK_UserAttendance_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID]) ON DELETE SET NULL ON UPDATE SET NULL;

GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [dbo].[UserAttendance] WITH CHECK CHECK CONSTRAINT [FK_UserAttendance_UserSeason];


GO
PRINT N'Update complete.';


GO
