UPDATE [dbo].[User]
SET [Mode] = 0
 
GO
PRINT N'Altering [dbo].[Group]...';


GO
ALTER TABLE [dbo].[Group]
    ADD [IsPhantom] BIT DEFAULT 0 NOT NULL;


GO
PRINT N'Altering [dbo].[User]...';


GO
ALTER TABLE [dbo].[User]
    ADD [IsPhantom] BIT DEFAULT 0 NOT NULL,
        [Todo]      INT DEFAULT 0 NOT NULL,
		[TutorialStep] INT DEFAULT 1 NOT NULL;


GO
PRINT N'Update complete.';

