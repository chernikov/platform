CREATE TABLE [dbo].[Post] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [Header]    NVARCHAR (50)  NOT NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Post_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

