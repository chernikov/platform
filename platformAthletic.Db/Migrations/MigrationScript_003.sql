INSERT INTO [dbo].[Sport]  ([Name])
VALUES ('Football');

GO

UPDATE [dbo].[FieldPosition] 
SET [SportID] = 1

GO 

INSERT INTO [dbo].[UserFieldPosition] ([UserID], [SportID], [FieldPositionID])
SELECT ID, 1 as SportID, FieldPositionID
FROM [dbo].[User] 

GO 

UPDATE [dbo].[SBCValue] 
SET [SportID] = 1

GO