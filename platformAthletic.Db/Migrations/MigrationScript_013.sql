PRINT N'Creating [dbo].[UserVideo]...';


GO
CREATE TABLE [dbo].[UserVideo] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [Header]    NVARCHAR (500) NOT NULL,
    [VideoUrl]  NVARCHAR (500) NOT NULL,
    [VideoCode] NVARCHAR (MAX) NOT NULL,
    [Preview]   NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_UserVideo] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating unnamed constraint on [dbo].[UserVideo]...';


GO
ALTER TABLE [dbo].[UserVideo]
    ADD DEFAULT '' FOR [Preview];


GO
PRINT N'Creating [dbo].[FK_UserVideo_User]...';


GO
ALTER TABLE [dbo].[UserVideo] WITH NOCHECK
    ADD CONSTRAINT [FK_UserVideo_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);


GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [dbo].[UserVideo] WITH CHECK CHECK CONSTRAINT [FK_UserVideo_User];


GO
PRINT N'Update complete.';


GO
