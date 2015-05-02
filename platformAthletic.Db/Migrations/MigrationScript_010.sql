CREATE INDEX UserAttendance_UserID
ON UserAttendance (UserID)

CREATE INDEX UserAttendance_AddedDate
ON UserAttendance (AddedDate)


CREATE INDEX UserAttendance_AddedDateUserID
ON UserAttendance (UserID, AddedDate)