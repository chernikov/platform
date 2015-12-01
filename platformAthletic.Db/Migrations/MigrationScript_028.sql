DROP TABLE  dbo.DelayedJob

CREATE TABLE dbo.DelayedJob
(
	[ID] int NOT NULL IDENTITY (1, 1),
	[Email] nvarchar(150) NOT NULL,
	[Password] nvarchar(50) NOT NULL,
	[Coach] nvarchar(1100) NOT NULL,
	[Subject] nvarchar(150) NOT NULL,
	[Type] int NOT NULL DEFAULT 0, 
	[Status] NCHAR(150) NULL,
	CONSTRAINT [PK_DelayedJob] PRIMARY KEY CLUSTERED ([ID] ASC)
) 