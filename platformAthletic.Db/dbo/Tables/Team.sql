CREATE TABLE [dbo].[Team] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [UserID]         INT            NOT NULL,
    [Name]           NVARCHAR (500) NOT NULL,
    [LogoPath]       NVARCHAR (150) NULL,
    [StateID]        INT            NOT NULL,
	[SchoolID]		 INT            NULL,
    [PrimaryColor]   NVARCHAR (10)  NOT NULL,
    [SecondaryColor] NVARCHAR (10)  NOT NULL,
    [SBCControl]     INT            NOT NULL,
	[MaxCount] INT DEFAULT 100 NOT NULL,
    CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Team_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([ID]),
	CONSTRAINT [FK_Team_School] FOREIGN KEY ([SchoolID]) REFERENCES [dbo].[School] ([ID]),
    CONSTRAINT [FK_Team_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

