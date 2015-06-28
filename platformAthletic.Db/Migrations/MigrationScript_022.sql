/*
The column [dbo].[Team].[SchoolID] is being dropped, data loss could occur.
*/
PRINT N'Dropping [dbo].[FK_Team_School]...';


GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [FK_Team_School];


GO
PRINT N'Altering [dbo].[Team]...';


GO
ALTER TABLE [dbo].[Team] DROP COLUMN [SchoolID];


GO
PRINT N'Update complete.';


GO
