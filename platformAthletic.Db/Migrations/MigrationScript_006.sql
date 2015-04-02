
GO
ALTER TABLE [dbo].[UserFieldPosition] WITH NOCHECK
    ADD CONSTRAINT [FK_UserFieldPosition_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);


GO
PRINT N'Checking existing data against newly created constraints';



GO
ALTER TABLE [dbo].[UserFieldPosition] WITH CHECK CHECK CONSTRAINT [FK_UserFieldPosition_User];


GO
PRINT N'Update complete.';