
GO
PRINT N'Rename refactoring operation with key 5feaa22d-f57b-4ede-b55a-3174bf7a5ccd is skipped, element [dbo].[Post].[Preview] (SqlSimpleColumn) will not be renamed to VideoPreview';


GO
PRINT N'Dropping [dbo].[FK_UserFieldPosition_User]...';


GO
ALTER TABLE [dbo].[UserFieldPosition] DROP CONSTRAINT [FK_UserFieldPosition_User];


GO
PRINT N'Creating [dbo].[FK_UserFieldPosition_User]...';


GO
ALTER TABLE [dbo].[UserFieldPosition] WITH NOCHECK
    ADD CONSTRAINT [FK_UserFieldPosition_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
-- Refactoring step to update target server with deployed transaction logs
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5feaa22d-f57b-4ede-b55a-3174bf7a5ccd')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5feaa22d-f57b-4ede-b55a-3174bf7a5ccd')

GO

GO
PRINT N'Checking existing data against newly created constraints';



GO
ALTER TABLE [dbo].[UserFieldPosition] WITH CHECK CHECK CONSTRAINT [FK_UserFieldPosition_User];


GO
PRINT N'Update complete.';


GO
