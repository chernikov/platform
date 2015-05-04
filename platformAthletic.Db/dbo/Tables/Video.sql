CREATE TABLE [dbo].[Video] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [TrainingID] INT            NULL,
    [Header]     NVARCHAR (500) NOT NULL,
    [Text]       NVARCHAR (MAX) NULL,
    [VideoUrl]   NVARCHAR (500) NOT NULL,
    [VideoCode]  NVARCHAR (MAX) NOT NULL,
    [Preview]    NVARCHAR(150) NOT NULL DEFAULT '', 
    CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Video_Training] FOREIGN KEY ([TrainingID]) REFERENCES [dbo].[Training] ([ID])
);

