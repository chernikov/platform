

GO
PRINT N'Dropping [dbo].[FK_UserFieldPosition_FieldPosition]...';


GO
ALTER TABLE [dbo].[UserFieldPosition] DROP CONSTRAINT [FK_UserFieldPosition_FieldPosition];


GO
PRINT N'Creating [dbo].[FK_UserFieldPosition_FieldPosition]...';


GO
ALTER TABLE [dbo].[UserFieldPosition] WITH NOCHECK
    ADD CONSTRAINT [FK_UserFieldPosition_FieldPosition] FOREIGN KEY ([FieldPositionID]) REFERENCES [dbo].[FieldPosition] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [dbo].[UserFieldPosition] WITH CHECK CHECK CONSTRAINT [FK_UserFieldPosition_FieldPosition];


GO
PRINT N'Update complete.';


GO
