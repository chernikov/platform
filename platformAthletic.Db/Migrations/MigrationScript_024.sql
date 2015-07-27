UPDATE [dbo].[User]
SET IndividualStateID = 1
WHERE ID IN (
SELECT u.ID
  FROM [dbo].[User] u
  JOIN [dbo].[UserRole] ur ON ur.UserID = u.ID
  WHERE u.IndividualStateID IS NULL AND u.PlayerOfTeamID IS NULL AND u.AssistantOfTeamID IS NULL AND ur.RoleID = 4)