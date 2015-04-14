using System;
using System.Collections.Generic;
using System.Linq;
using platformAthletic.Tools;

namespace platformAthletic.Model
{
    public partial class SqlRepository
    {
        public IQueryable<User> Users
        {
            get
            {
                return Db.Users.Where(p => !p.IsDeleted);
            }
        }

        public IQueryable<User> AllUsers
        {
            get
            {
                return Db.Users;
            }
        }


        public bool CreateUser(User instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.LastVisitDate = DateTime.Now;
                instance.PaidTill = DateTime.Now.AddDays(1);
                instance.ActivatedLink = StringExtension.GenerateNewFile();
                Db.Users.InsertOnSubmit(instance);
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool CreateAdminUser(User instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.LastVisitDate = DateTime.Now;
                if (string.IsNullOrWhiteSpace(instance.ActivatedLink))
                {
                    instance.ActivatedLink = string.Empty;
                }
                Db.Users.InsertOnSubmit(instance);
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateAdminUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Email = instance.Email;
                cache.FirstName = instance.FirstName;
                cache.LastName = instance.LastName;
                cache.PhoneNumber = instance.PhoneNumber;
                cache.Password = instance.Password;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public User GetUser(string email)
        {
            var user = Db.Users.FirstOrDefault(p => string.Compare(p.Email, email, true) == 0);
            /*  if (user != null)
              {
                  user.LastVisitDate = DateTime.Now;
                  Db.Users.Context.SubmitChanges();
              }*/
            return user;
        }

        public bool OnlineUser(int idUser)
        {
            var user = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (user != null)
            {
                user.LastVisitDate = DateTime.Now;
                Db.Users.Context.SubmitChanges();
            }
            return true;
        }

        public User Login(string email, string password)
        {
            return Db.Users.FirstOrDefault(p => string.Compare(p.Email, email, true) == 0 && p.Password == password);
        }


        public bool UpdateUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.AvatarPath = instance.AvatarPath;
                cache.FirstName = instance.FirstName;
                cache.LastName = instance.LastName;
                cache.PhoneNumber = instance.PhoneNumber;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateManageUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.FirstName = instance.FirstName;
                cache.LastName = instance.LastName;
                cache.PhoneNumber = instance.PhoneNumber;
                cache.Email = instance.Email;
                cache.GroupID = instance.GroupID;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool SetUserColors(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.PrimaryColor = instance.PrimaryColor;
                cache.SecondaryColor = instance.SecondaryColor;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdatePaidTillUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.PaidTill = instance.PaidTill;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool ActivateUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.ActivatedDate = DateTime.Now;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ResendRegister(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.LoginInfoSent = DateTime.Now;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }


        public bool ChangePassword(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Password = instance.Password;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool SetSbcValue(int idUser, SBCValue.SbcType type, double value, DateTime? addedDate = null)
        {
            var instance = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null)
            {
                switch (type)
                {
                    case SBCValue.SbcType.Squat:
                        instance.Squat = value;
                        break;
                    case SBCValue.SbcType.Bench:
                        instance.Bench = value;
                        break;
                    case SBCValue.SbcType.Clean:
                        instance.Clean = value;
                        break;
                }
                SaveSBCValue(idUser, instance.Squat, instance.Bench, instance.Clean, addedDate);
            }
            return false;
        }



        public bool ChangeSbcValue(int idUser, SBCValue.SbcType type, double difference)
        {
            var instance = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null)
            {
                switch (type)
                {
                    case SBCValue.SbcType.Squat:
                        instance.Squat += difference;
                        break;
                    case SBCValue.SbcType.Bench:
                        instance.Bench +=difference;
                        break;
                    case SBCValue.SbcType.Clean:
                        instance.Clean +=difference;
                        break;
                }
                SaveSBCValue(idUser, instance.Squat, instance.Bench, instance.Clean);
            }
            return false;
        }
        public bool SetUserField(int idUser, User.FieldType fieldType, string value)
        {
            var instance = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null)
            {
                double dValue = 0;
                switch (fieldType)
                {
                    case User.FieldType.Year:
                        int year = 0;
                        if (Int32.TryParse(value, out year))
                        {
                            instance.Year = year;
                        }
                        break;
                    case User.FieldType.Squat:
                        if (Double.TryParse(value, out dValue))
                        {
                            SetSbcValue(idUser, SBCValue.SbcType.Squat, dValue);
                        }
                        break;
                    case User.FieldType.Bench:
                        if (Double.TryParse(value, out dValue))
                        {
                            SetSbcValue(idUser, SBCValue.SbcType.Bench, dValue);
                        }
                        break;
                    case User.FieldType.Clean:
                        if (Double.TryParse(value, out dValue))
                        {
                            SetSbcValue(idUser, SBCValue.SbcType.Clean, dValue);
                        }
                        break;
                    case User.FieldType.Height:
                        instance.Height = value;
                        break;
                    case User.FieldType.Weight:
                        instance.Weight = value;
                        break;
                    case User.FieldType.BodyFat:
                        instance.BodyFat = value;
                        break;
                    case User.FieldType._40YardDash:
                        if (Double.TryParse(value, out dValue))
                        {
                            instance._40YardDash = dValue;
                        }
                        break;
                    case User.FieldType.Vertical:
                        if (Double.TryParse(value, out dValue))
                        {
                            instance.Vertical = dValue;
                        }
                        break;
                    case User.FieldType._3Cone:
                        if (Double.TryParse(value, out dValue))
                        {
                            instance._3Cone = dValue;
                        }
                        break;
                    case User.FieldType.TDrill:
                        if (Double.TryParse(value, out dValue))
                        {
                            instance.TDrill = dValue;
                        }
                        break;
                }
                Db.Users.Context.SubmitChanges();
            }
            return false;
        }

        //TODO SetFieldPosition 
        public bool SetFieldPosition(int idUser, int fieldPositionID)
        {
            var instance = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null)
            {
                //instance.FieldPositionID = fieldPositionID;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool SFieldPosition(int idUser, int fieldPositionID)
        {
            var instance = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null)
            {
                //instance.FieldPositionID = fieldPositionID;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool SetAttendance(int idUser, bool attendance, int idUserSeason, DateTime? date = null)
        {
            var attendanceDate = date ?? DateTime.Now.Date;
            var exist = Db.UserAttendances.FirstOrDefault(p => p.UserID == idUser && p.AddedDate == attendanceDate.Date);

            if (exist != null)
            {
                if (attendance)
                {
                    return true;
                }
                else
                {
                    Db.UserAttendances.DeleteOnSubmit(exist);
                    Db.UserAttendances.Context.SubmitChanges();
                    return true;
                }
            }
            else
            {
                if (attendance)
                {
                    var userAttendance = new UserAttendance
                    {
                        UserID = idUser,
                        UserSeasonID = idUserSeason,
                        AddedDate = attendanceDate.Date
                    };
                    Db.UserAttendances.InsertOnSubmit(userAttendance);
                    Db.UserAttendances.Context.SubmitChanges();
                }
                return true;
            }
        }

        public bool VisitGettingStarted(int idUser)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (cache != null)
            {
                cache.VisitGettingStartedPage = true;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool PurgeUser(int idUser)
        {
            var instance = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null)
            {
                if (instance.OwnTeam != null && instance.OwnTeam.SBCValues.Any())
                {
                    foreach (var sbcValue in instance.OwnTeam.SBCValues.ToList())
                    {
                        sbcValue.TeamID = null;
                    }
                    Db.SBCValues.Context.SubmitChanges();
                }

                if (instance.OwnTeam != null && instance.OwnTeam.Users.Any())
                {
                    var players = instance.OwnTeam.Users;
                    Db.Users.DeleteAllOnSubmit(players);
                    Db.Users.Context.SubmitChanges();
                }

                var userSeasons = instance.UserSeasons.ToList();
                var schedules = userSeasons.SelectMany(p => p.Schedules).ToList();
                var personalSchedules = userSeasons.SelectMany(p => p.PersonalSchedules).ToList();
                Db.Schedules.DeleteAllOnSubmit(schedules);
                Db.PersonalSchedules.DeleteAllOnSubmit(personalSchedules);
                Db.UserSeasons.DeleteAllOnSubmit(userSeasons);
                Db.Users.DeleteOnSubmit(instance);
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveUser(int idUser)
        {
            var instance = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null)
            {
                instance.IsDeleted = true;
                Db.Users.Context.SubmitChanges();
            }
            return false;
        }


        public bool ChangeGroup(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.GroupID = instance.GroupID;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ResetAttendance(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.AttendanceStartDate = DateTime.Now;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ResetProgress(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.ProgressStartDate = DateTime.Now;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool SetProgressDate(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.ProgressStartDate = instance.ProgressStartDate;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateFullUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Email = instance.Email;
                cache.AvatarPath = instance.AvatarPath;
                cache.FirstName = instance.FirstName;
                cache.LastName = instance.LastName;
                cache.PhoneNumber = instance.PhoneNumber;
                cache.PaidTill = instance.PaidTill;
                cache.PlayerOfTeamID = instance.PlayerOfTeamID;
                cache.LevelID = instance.LevelID;
                cache.Squat = instance.Squat;
                cache.Bench = instance.Bench;
                cache.Clean = instance.Clean;
                cache.Height = instance.Height;
                cache.Weight = instance.Weight;
                cache.BodyFat = instance.BodyFat;
                cache._40YardDash = instance._40YardDash;
                cache.Vertical = instance.Vertical;
                cache._3Cone = instance._3Cone;
                cache.TDrill = instance.TDrill;
                cache.PrimaryColor = instance.PrimaryColor;
                cache.SecondaryColor = instance.SecondaryColor;
                Db.Users.Context.SubmitChanges();
                if (cache.InRoles("coach"))
                {
                    var team = cache.OwnTeam;
                    team.PrimaryColor = instance.PrimaryColor;
                    team.SecondaryColor = instance.SecondaryColor;
                    UpdateTeam(team);
                }
                return true;
            }
            return false;
        }
    }
}