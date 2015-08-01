

GO
ALTER TABLE [dbo].[TrainingDay] DROP CONSTRAINT [FK_TrainingDay_Macrocycle];


GO
PRINT N'Dropping [dbo].[FK_School_State]...';


GO
ALTER TABLE [dbo].[School] DROP CONSTRAINT [FK_School_State];


GO
PRINT N'Dropping [dbo].[__RefactorLog]...';


GO
DROP TABLE [dbo].[__RefactorLog];


GO
PRINT N'Dropping [dbo].[School]...';


GO
DROP TABLE [dbo].[School];


GO
PRINT N'Creating [dbo].[FK_TrainingDay_Macrocycle]...';


GO
ALTER TABLE [dbo].[TrainingDay] WITH NOCHECK
    ADD CONSTRAINT [FK_TrainingDay_Macrocycle] FOREIGN KEY ([MacrocycleID]) REFERENCES [dbo].[Macrocycle] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [dbo].[TrainingDay] WITH CHECK CHECK CONSTRAINT [FK_TrainingDay_Macrocycle];


GO
PRINT N'Update complete.';


GO
