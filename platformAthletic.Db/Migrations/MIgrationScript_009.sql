
PRINT N'Dropping [dbo].[FK_SBCValue_FieldPosition]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_FieldPosition];


GO
PRINT N'Altering [dbo].[SBCValue]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP COLUMN [FieldPositionID];


GO
PRINT N'Update complete.';


GO

DELETE FROM [dbo].[SBCValue]
WHERE UserID IS NULL

GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_User];


GO
PRINT N'Altering [dbo].[SBCValue]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP COLUMN [FirstName], COLUMN [LastName];


GO
ALTER TABLE [dbo].[SBCValue] ALTER COLUMN [UserID] INT NOT NULL;


GO
PRINT N'Creating [dbo].[FK_SBCValue_User]...';


GO
ALTER TABLE [dbo].[SBCValue] WITH NOCHECK
    ADD CONSTRAINT [FK_SBCValue_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Checking existing data against newly created constraints';



GO
ALTER TABLE [dbo].[SBCValue] WITH CHECK CHECK CONSTRAINT [FK_SBCValue_User];


GO
PRINT N'Update complete.';



GO
ALTER TABLE [dbo].[SBCValue] DROP COLUMN [SportID];

GO
PRINT N'Dropping [dbo].[FK_SBCValue_Sport]...';


GO
ALTER TABLE [dbo].[SBCValue] DROP CONSTRAINT [FK_SBCValue_Sport];


GO
PRINT N'Update complete.';


GO
