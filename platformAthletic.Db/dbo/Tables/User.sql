﻿CREATE TABLE [dbo].[User] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
	[IndividualStateID]       INT            NULL, 
    [Email]                   NVARCHAR (150) NOT NULL,
    [Password]                NVARCHAR (50)  NOT NULL,
    [AddedDate]               DATETIME       NOT NULL,
    [ActivatedDate]           DATETIME       NULL,
    [ActivatedLink]           NVARCHAR (50)  NOT NULL,
    [LastVisitDate]           DATETIME       NOT NULL,
    [AvatarPath]              NVARCHAR (150) NULL,
    [FirstName]               NVARCHAR (500) NULL,
    [LastName]                NVARCHAR (500) NULL,
    [PaidTill]                DATETIME       NULL,
    [PhoneNumber]             NVARCHAR (50)  NULL,
    [PlayerOfTeamID]          INT            NULL,
	[AssistantOfTeamID]       INT            NULL,
    [GroupID]                 INT            NULL,
    [VisitGettingStartedPage] BIT            NULL,
    [Year]                    INT            NULL,
    [Squat]                   FLOAT (53)     NOT NULL,
    [Bench]                   FLOAT (53)     NOT NULL,
    [Clean]                   FLOAT (53)     NOT NULL,
    [Height]                  NVARCHAR (50)  NULL,
    [Weight]                  NVARCHAR (50)  NULL,
    [BodyFat]                 NVARCHAR (50)  NULL,
    [_40YardDash]             FLOAT (53)     NULL,
    [Vertical]                FLOAT (53)     NULL,
    [_3Cone]                  FLOAT (53)     NULL,
    [TDrill]                  FLOAT (53)     NULL,
    [PrimaryColor]            NVARCHAR (50)  NULL,
    [SecondaryColor]          NVARCHAR (50)  NULL,
    [LoginInfoSent]           DATETIME       NULL,
    [AttendanceStartDate]     DATETIME       NULL,
    [ProgressStartDate]       DATETIME       NULL,
	[Birthday]				  DATETIME		 NULL,
    [Gender]				  BIT			 NOT NULL DEFAULT 1, 
    [LevelID]				  INT			 NULL, 
    [GradYear]				  INT		     NULL, 
	[PublicLevel]             INT            NOT NULL DEFAULT 2, 
	[Mode]					  INT		     NOT NULL DEFAULT 0,
    [IsDeleted]               BIT            NOT NULL DEFAULT 0, 
    [IsPhantom]				  BIT			 NOT NULL DEFAULT 0, 
	[Todo]					  INT			 NOT NULL DEFAULT 0, 
	[TutorialStep]  		  INT			 NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_User_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]),
    CONSTRAINT [FK_User_Team] FOREIGN KEY ([PlayerOfTeamID]) REFERENCES [dbo].[Team] ([ID]),
	CONSTRAINT [FK_User_Team2] FOREIGN KEY ([AssistantOfTeamID]) REFERENCES [dbo].[Team] ([ID]),
	CONSTRAINT [FK_User_Level] FOREIGN KEY ([LevelID]) REFERENCES [dbo].[Level] ([ID]),
	CONSTRAINT [FK_User_IndividualState] FOREIGN KEY ([IndividualStateID]) REFERENCES [dbo].[State] ([ID])
);

