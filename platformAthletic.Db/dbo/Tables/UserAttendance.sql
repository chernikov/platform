CREATE TABLE [dbo].[UserAttendance] (
    [ID]           INT      IDENTITY (1, 1) NOT NULL,
    [UserSeasonID] INT      NOT NULL,
    [UserID]       INT      NOT NULL,
    [AddedDate]    DATETIME NOT NULL,
    CONSTRAINT [PK_UserAttendance] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserAttendance_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserAttendance_UserSeason] FOREIGN KEY ([UserSeasonID]) REFERENCES [dbo].[UserSeason] ([ID])
);

