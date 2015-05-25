
GO
ALTER TABLE [dbo].[Post]
    ADD [TitleImagePath] NVARCHAR (150) DEFAULT '' NOT NULL;


GO
PRINT N'Rename refactoring operation with key 5feaa22d-f57b-4ede-b55a-3174bf7a5ccd is skipped, element [dbo].[Post].[Preview] (SqlSimpleColumn) will not be renamed to VideoPreview';


GO
PRINT N'Altering [dbo].[Post]...';


GO
ALTER TABLE [dbo].[Post]
    ADD [Promoted]     BIT            DEFAULT 0 NOT NULL,
        [IsVideo]      BIT            DEFAULT 0 NOT NULL,
        [VideoUrl]     NVARCHAR (150) NULL,
        [VideoCode]    NVARCHAR (MAX) NULL,
        [VideoPreview] NVARCHAR (150) NULL,
        [CountOfView]  INT            DEFAULT 0 NOT NULL;


GO
-- Refactoring step to update target server with deployed transaction logs
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5feaa22d-f57b-4ede-b55a-3174bf7a5ccd')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5feaa22d-f57b-4ede-b55a-3174bf7a5ccd')