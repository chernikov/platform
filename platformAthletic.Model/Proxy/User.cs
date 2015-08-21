using System;
using System.Collections.Generic;
using System.Linq;

namespace platformAthletic.Model
{
    public partial class User
    {
        public enum FieldType
        {
            Year = 0x01,
            Squat = 0x02,
            Bench = 0x03,
            Clean = 0x04,
            Height = 0x05,
            Weight = 0x06,
            BodyFat = 0x07,
            _40YardDash = 0x08,
            Vertical = 0x09,
            _3Cone = 0x0A,
            TDrill = 0x0B
        }

        public enum PublicLevelEnum
        {
            Private = 0x00,
            Limited = 0x01,
            Public = 0x02
        }

        public enum ModeEnum
        {
            Normal = 0x00,
            Tutorial = 0x01,
            Test = 0x02,
            Todo = 0x03
        }

        [Flags]
        public enum TodoEnum
        {
            AddEquipment = 0x01,
            ViewWorkOut = 0x02,
            ConfirmTrainingStartDate = 0x04,
            AddPlayers = 0x08,
            EnterMaxes = 0x10,
            Leaderboard = 0x20,
            UploadVideo = 0x40,
        }


        public static List<Level> GradYearLevels = new List<Level>{
            new Level() {
                ID = 2, 
                Name = "High School"
            },
            new Level() {
                ID = 3, 
                Name = "College"
            },
        };


        public Team TeamOfPlay
        {
            get
            {
                return Team;
            }

            set
            {
                Team = value;
            }
        }

        public Team TeamOfAssistance
        {
            get
            {
                return Team1;
            }

            set
            {
                Team1 = value;
            }
        }


        public bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }

            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in rolesArray)
            {
                if (role == "payed")
                {
                    return RealPaidTill.HasValue && DateTime.Now <= RealPaidTill;
                }
                var hasRole = UserRoles.Any(p => string.Compare(p.Role.Code, role, true) == 0);
                if (hasRole)
                {
                    return true;
                }
            }
            return false;
        }


        public IQueryable<FieldPosition> FieldPositions
        {
            get
            {
                return UserFieldPositions.Select(p => p.FieldPosition).AsQueryable();
            }
        }

        public bool IsActivated
        {
            get { return ActivatedDate.HasValue; }
        }

        public string FullAvatarPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AvatarPath))
                {
                    return "/Media/images/no-userpic.png";
                }
                return AvatarPath;
            }
        }

        public Team OwnTeam
        {
            get
            {
                if (InRoles("assistant"))
                {
                    return TeamOfAssistance;
                }
                return Teams.FirstOrDefault();
            }
        }

        public BillingInfo BillingInfo
        {
            get
            {
                return BillingInfos.FirstOrDefault();
            }
        }

        public Invoice Invoice
        {
            get
            {
                return Invoices.FirstOrDefault();
            }
        }

        public string RealPrimaryColor
        {
            get
            {
                if (InRoles("individual") || InRoles("admin"))
                {
                    return PrimaryColor ?? string.Empty;
                }
                else if (OwnTeam != null)
                {
                    return OwnTeam.PrimaryColor ?? string.Empty;
                }
                else if (TeamOfPlay != null)
                {
                    return TeamOfPlay.PrimaryColor ?? string.Empty;
                }
                return string.Empty;
            }
        }

        public string RealSecondaryColor
        {
            get
            {
                if (InRoles("individual") || InRoles("admin"))
                {
                    return SecondaryColor ?? string.Empty;
                }
                else if (OwnTeam != null)
                {
                    return OwnTeam.SecondaryColor ?? string.Empty;
                }
                else if (TeamOfPlay != null)
                {
                    return TeamOfPlay.SecondaryColor ?? string.Empty;
                }
                return string.Empty;
            }
        }

        public UserSeason CurrentSeason
        {
            get
            {
                //if player of team
                if ((TeamOfPlay != null && ID != TeamOfPlay.UserID) || (TeamOfAssistance != null && ID != TeamOfAssistance.UserID))
                {
                    var coach = TeamOfPlay != null ? TeamOfPlay.User : TeamOfAssistance.User;
                    if (GroupID != null)
                    {
                        var currentSeason = coach.SeasonByDateAndGroup(SqlSingleton.sqlRepository.CurrentDateTime, GroupID.Value);
                        if (currentSeason == null)
                        {
                            return coach.SeasonByDateAndGroup(SqlSingleton.sqlRepository.CurrentDateTime);
                        }
                        return currentSeason;
                    }
                    else
                    {
                        return coach.SeasonByDateAndGroup(SqlSingleton.sqlRepository.CurrentDateTime);
                    }
                }
                else
                {
                    //user is coach
                    return SeasonByDateAndGroup(SqlSingleton.sqlRepository.CurrentDateTime);
                }
            }
        }

        public UserSeason SeasonByDateAndGroup(DateTime dateTime, int? groupID = null, bool getTeam = false)
        {
            var season = UserSeasons.OrderByDescending(p => p.StartDay).ThenByDescending(p => ID).FirstOrDefault(p => p.StartDay <= dateTime && p.GroupID == groupID);
            if (season == null && groupID == null)
            {
                season = UserSeasons.Where(p => p.StartDay >= dateTime && p.GroupID == groupID).OrderBy(p => p.StartDay).ThenByDescending(p => ID).FirstOrDefault();
            }
            if (season == null && getTeam)
            {
                return SeasonByDateAndGroup(dateTime, null);
            }
            return season;
        }


        public UserSeason NextSeason
        {
            get
            {
                if (TeamOfPlay != null && ID != TeamOfPlay.UserID)
                {
                    return TeamOfPlay.NextSeason;
                }
                else
                {
                    var currentSeason = CurrentSeason;
                    if (currentSeason != null)
                    {
                        return UserSeasons.Where(p => p.StartDay > currentSeason.StartDay).FirstOrDefault();
                    }

                }
                return null;
            }
        }

        public string TeamLogo
        {
            get
            {

                if (OwnTeam != null)
                {
                    return OwnTeam.LogoPath;
                }
                if (TeamOfPlay != null)
                {
                    return TeamOfPlay.LogoPath;
                }
                return null;
            }
        }

        public bool InTrainingSeason
        {
            get
            {
                if (CurrentSeason != null)
                {
                    return CurrentSeason.StartDay <= SqlSingleton.sqlRepository.CurrentDateTime &&
                        CurrentSeason.StartDay.AddDays(CurrentSeason.Season.DaysLength) >= SqlSingleton.sqlRepository.CurrentDateTime;
                }
                return false;
            }
        }

        public bool InTrainingNextSeason
        {
            get
            {
                if (NextSeason != null)
                {
                    return NextSeason.StartDay <= SqlSingleton.sqlRepository.CurrentDateTime &&
                    NextSeason.StartDay.AddDays(NextSeason.Season.DaysLength) >= SqlSingleton.sqlRepository.CurrentDateTime;
                }
                return false;
            }
        }

        public bool HasAttendanceToday
        {
            get
            {
                return UserAttendances.Any(p => p.AddedDate == SqlSingleton.sqlRepository.CurrentDateTime.Date);
            }
        }

        public bool HasAttendance(DateTime date)
        {
            return UserAttendances.Any(p => p.AddedDate == date.Date);
        }

        public double SbcSum
        {
            get
            {
                return Squat + Bench + Clean;
            }
        }

        public bool AnyUserEquipments
        {
            get
            {
                return UserEquipments.Any();
            }
        }

        public bool AnyUserPlayer
        {
            get
            {
                return OwnTeam.Players.Any();
            }
        }

        public bool CanEditSBC(User user)
        {
            if (user != null)
            {
                if (user.OwnTeam != null && (user.OwnTeam.ID == PlayerOfTeamID || user.OwnTeam.ID == AssistantOfTeamID))
                {
                    return true;
                }
                if (InRoles("individual") && user.ID == ID)
                {
                    return true;
                }
                if (user.TeamOfPlay != null && user.TeamOfPlay.SBCControl == (int)Team.SBCControlType.CoachAndPlayer && user.ID == ID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanBeDeleted(User user)
        {
            if (user != null)
            {
                if (user.OwnTeam != null && (user.OwnTeam.ID == PlayerOfTeamID || user.OwnTeam.ID == AssistantOfTeamID))
                {
                    return true;
                }
                if (InRoles("individual") && user.ID == ID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanEditAttendance(User user)
        {
            if (user != null)
            {
                if (user.OwnTeam != null && (user.OwnTeam.ID == PlayerOfTeamID || user.OwnTeam.ID == AssistantOfTeamID))
                {
                    return true;
                }
                if (InRoles("individual") && user.ID == ID)
                {
                    return true;
                }
                if (user.TeamOfPlay != null && user.TeamOfPlay.SBCAttendance == (int)Team.SBCControlType.CoachAndPlayer && user.ID == ID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanEditOwnData(User user)
        {
            if (user != null)
            {
                if (user.ID == ID)
                {
                    return true;
                }
                //if (InRoles("player") && user.OwnTeam != null && user.OwnTeam.ID == PlayerOfTeamID)
                //{
                //    return true;
                //}
            }
            return false;
        }

        public bool CanViewWorkOut(User user)
        {
            if (user != null)
            {
                if (user.ID == ID)
                {
                    return true;
                }
            }
            return false;
        }


        public List<Equipment> Equpments
        {
            get
            {
                if (InRoles("player"))
                {
                    return TeamOfPlay.User.UserEquipments.Select(p => p.Equipment).ToList();
                }
                else
                {
                    return UserEquipments.Select(p => p.Equipment).ToList();
                }
            }
        }

        public string Role
        {
            get
            {
                return string.Join(", ", UserRoles.Select(p => p.Role.Name));
            }
        }

        public IEnumerable<string> PromoCodes
        {
            get
            {
                return PaymentDetails.Where(p => !string.IsNullOrWhiteSpace(p.ReferralCode)).Select(p => p.ReferralCode).ToList();
            }
        }

        public DateTime? RealPaidTill
        {
            get
            {
                if (InRoles("coach,individual"))
                {
                    return PaidTill;
                }
                if (InRoles("player"))
                {
                    return TeamOfPlay.User.PaidTill;
                }
                if (InRoles("assistant"))
                {
                    return OwnTeam.User.PaidTill;
                }
                return null;
            }
        }

        public string InitialAndLastName
        {
            get
            {
                return (FirstName.Substring(0, 1) + ". " + LastName).ToUpper();
            }
        }

        public bool IsWeekAvaiable
        {
            get
            {
                if (CurrentSeason != null)
                {
                    int numberOfWeek = (int)(((int)((SqlSingleton.sqlRepository.CurrentDateTime - CurrentSeason.StartDay).TotalDays) / 7));
                    int totalWeeks = CurrentSeason.Season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).Where(p => p.Number != null).Count();
                    numberOfWeek = numberOfWeek % totalWeeks + 1;
                    return CurrentSeason.Season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).Any(p => p.Number == numberOfWeek);
                }
                return false;
            }
        }

        public DateTime? FullAttendanceStartDate
        {
            get
            {
                if (AttendanceStartDate.HasValue)
                {
                    return AttendanceStartDate.Value;
                }
                if (CurrentSeason != null)
                {
                    return CurrentSeason.StartDay;
                }
                return null;
            }
        }

        public DateTime? FullProgressStartDate
        {
            get
            {
                if (ProgressStartDate.HasValue)
                {
                    return ProgressStartDate.Value;
                }
                if (CurrentSeason != null)
                {
                    return CurrentSeason.StartDay;
                }
                return null;
            }
        }

        public IEnumerable<UserFieldPosition> SubPositions
        {
            get
            {
                return UserFieldPositions.AsEnumerable();
            }
        }

        public SBCValue SBCHistory(DateTime date, SBCValue.SbcType? restrict = null)
        {
            if (!restrict.HasValue)
            {
                return SBCValues.Where(p => p.AddedDate <= date).OrderBy(p => p.ID).LastOrDefault();
            }
            else
            {
                switch (restrict.Value)
                {
                    case SBCValue.SbcType.Squat:
                        return SBCValues.Where(p => p.AddedDate <= date && p.Squat > 0).OrderBy(p => p.ID).LastOrDefault();
                    case SBCValue.SbcType.Bench:
                        return SBCValues.Where(p => p.AddedDate <= date && p.Bench > 0).OrderBy(p => p.ID).LastOrDefault();
                    case SBCValue.SbcType.Clean:
                        return SBCValues.Where(p => p.AddedDate <= date && p.Clean > 0).OrderBy(p => p.ID).LastOrDefault();
                }
            }
            return null;
        }

        public SBCValue SBCForward(DateTime date, DateTime endDate, SBCValue.SbcType? restrict = null)
        {
            if (!restrict.HasValue)
            {
                return SBCValues.Where(p => p.AddedDate <= date && p.AddedDate <= endDate).OrderBy(p => p.ID).LastOrDefault();
            }
            else
            {
                switch (restrict.Value)
                {
                    case SBCValue.SbcType.Squat:
                        return SBCValues.Where(p => p.AddedDate >= date && p.AddedDate <= endDate && p.Squat > 0).OrderBy(p => p.ID).FirstOrDefault();
                    case SBCValue.SbcType.Bench:
                        return SBCValues.Where(p => p.AddedDate >= date && p.AddedDate <= endDate && p.Bench > 0).OrderBy(p => p.ID).FirstOrDefault();
                    case SBCValue.SbcType.Clean:
                        return SBCValues.Where(p => p.AddedDate >= date && p.AddedDate <= endDate && p.Clean > 0).OrderBy(p => p.ID).FirstOrDefault();
                }
            }
            return null;
        }

       

        public SBCValue SBCFirstHistory(SBCValue.SbcType? restrict = null)
        {
            if (!restrict.HasValue)
            {
                return SBCValues.OrderBy(p => p.ID).FirstOrDefault();
            }
            else
            {
                switch (restrict.Value)
                {
                    case SBCValue.SbcType.Squat:
                        return SBCValues.Where(p => p.Squat > 0).OrderBy(p => p.ID).FirstOrDefault();
                    case SBCValue.SbcType.Bench:
                        return SBCValues.Where(p => p.Bench > 0).OrderBy(p => p.ID).FirstOrDefault();
                    case SBCValue.SbcType.Clean:
                        return SBCValues.Where(p => p.Clean > 0).OrderBy(p => p.ID).FirstOrDefault();
                }
            }
            return null;
        }


        private SBCValue _last12Week;

        public SBCValue Last12Week
        {
            get
            {
                if (_last12Week != null)
                {
                    return _last12Week;
                }
                var startWeek = SqlSingleton.sqlRepository.CurrentDateTime.AddDays(-(int)SqlSingleton.sqlRepository.CurrentDateTime.DayOfWeek).Date;
                var date12WeekAgo = startWeek.AddDays(-7 * 11);
                var sbc = SBCValues.Where(p => p.AddedDate <= date12WeekAgo).OrderBy(p => p.ID).LastOrDefault();
                while ((sbc == null || sbc.Equals(SBCValue.EmptySBCValue))  && date12WeekAgo <= startWeek)
                {
                    date12WeekAgo = date12WeekAgo.AddDays(7);
                    sbc = SBCValues.Where(p => p.AddedDate <= date12WeekAgo).OrderBy(p => p.ID).LastOrDefault();
                }
                _last12Week = sbc;
                return _last12Week;
            }
        }

        private int? _diffSquat12Week;
        public int DiffSquat12Week
        {
            get
            {
                if (_diffSquat12Week.HasValue)
                {
                    return _diffSquat12Week.Value;
                }
                var startWeek = SqlSingleton.sqlRepository.CurrentDateTime.AddDays(-(int)SqlSingleton.sqlRepository.CurrentDateTime.DayOfWeek).Date;
                var date12WeekAgo = startWeek.AddDays(-7 * 11);
                var firstSquatSbc = SBCValues.Where(p => p.AddedDate <= date12WeekAgo && p.Squat != 0).OrderBy(p => p.ID).LastOrDefault();
                while (firstSquatSbc == null && date12WeekAgo <= startWeek)
                {
                    date12WeekAgo = date12WeekAgo.AddDays(7);
                    firstSquatSbc = SBCValues.Where(p => p.AddedDate <= date12WeekAgo && p.Squat != 0).OrderBy(p => p.ID).LastOrDefault();
                }
                if (firstSquatSbc != null)
                {
                    _diffSquat12Week = (int)Squat - (int)firstSquatSbc.Squat;
                }
                else
                {
                    _diffSquat12Week = 0;
                }
                return _diffSquat12Week ?? 0;
            }
        }

        private int? _diffBench12Week;

        public int DiffBench12Week
        {
            get
            {
                if (_diffBench12Week.HasValue)
                {
                    return _diffBench12Week.Value;
                }
                var startWeek = SqlSingleton.sqlRepository.CurrentDateTime.AddDays(-(int)SqlSingleton.sqlRepository.CurrentDateTime.DayOfWeek).Date;
                var date12WeekAgo = startWeek.AddDays(-7 * 11);
                var firstBenchSbc = SBCValues.Where(p => p.AddedDate <= date12WeekAgo && p.Bench != 0).OrderBy(p => p.ID).LastOrDefault();
                while (firstBenchSbc == null && date12WeekAgo <= startWeek)
                {
                    date12WeekAgo = date12WeekAgo.AddDays(7);
                    firstBenchSbc = SBCValues.Where(p => p.AddedDate <= date12WeekAgo && p.Bench != 0).OrderBy(p => p.ID).LastOrDefault();
                }
                if (firstBenchSbc != null)
                {
                    _diffBench12Week = (int)Bench - (int)firstBenchSbc.Bench;
                }
                else
                {
                    _diffBench12Week = 0;
                }
                return _diffBench12Week ?? 0;

            }
        }

        private int? _diffClean12Week;

        public int DiffClean12Week
        {
            get
            {
                if (_diffClean12Week.HasValue)
                {
                    return _diffClean12Week.Value;
                }
                var startWeek = SqlSingleton.sqlRepository.CurrentDateTime.AddDays(-(int)SqlSingleton.sqlRepository.CurrentDateTime.DayOfWeek).Date;
                var date12WeekAgo = startWeek.AddDays(-7 * 11);
                var firstCleanSbc = SBCValues.Where(p => p.AddedDate <= date12WeekAgo && p.Clean != 0).OrderBy(p => p.ID).LastOrDefault();
                while (firstCleanSbc == null && date12WeekAgo <= startWeek)
                {
                    date12WeekAgo = date12WeekAgo.AddDays(7);
                    firstCleanSbc = SBCValues.Where(p => p.AddedDate <= date12WeekAgo && p.Clean != 0).OrderBy(p => p.ID).LastOrDefault();
                }
                if (firstCleanSbc != null)
                {
                    _diffClean12Week = (int)Clean - (int)firstCleanSbc.Clean;
                }
                else
                {
                    _diffClean12Week = 0;
                }
                return _diffClean12Week ?? 0;

            }
        }


        public int Total
        {
            get
            {
                return (int)(Squat + Bench + Clean);
            }
        }

        public string GenderStr
        {
            get
            {
                return Gender ? "M" : "F";
            }
        }

        public int? Age
        {
            get
            {
                if (Birthday != null)
                {
                    var today = DateTime.Today;
                    var birthday = Birthday.Value;
                    int age = today.Year - birthday.Year;
                    if (birthday > today.AddYears(-age)) age--;
                    return age;
                }
                return null;
            }
        }

        #region FieldPosition

        public UserFieldPosition FirstFieldPosition
        {
            get
            {
                return UserFieldPositions.FirstOrDefault();
            }
        }

        public UserFieldPosition SecondFieldPosition
        {
            get
            {
                return UserFieldPositions.Skip(1).FirstOrDefault();
            }
        }

        public UserFieldPosition ThirdFieldPosition
        {
            get
            {
                return UserFieldPositions.Skip(2).FirstOrDefault();
            }
        }
        #endregion

        #region AttendanceCount
        public int WeekAttendanceCount
        {
            get
            {
                int diff = SqlSingleton.sqlRepository.CurrentDateTime.DayOfWeek - DayOfWeek.Sunday;
                if (diff < 0)
                {
                    diff += 7;
                }
                var startWeek = SqlSingleton.sqlRepository.CurrentDateTime.AddDays(-1 * diff).Date;
                return UserAttendances.Count(p => p.AddedDate >= startWeek);
            }
        }

        public int MonthAttendanceCount
        {
            get
            {
                var startMonth = new DateTime(SqlSingleton.sqlRepository.CurrentDateTime.Year, SqlSingleton.sqlRepository.CurrentDateTime.Month, 1);
                return UserAttendances.Count(p => p.AddedDate >= startMonth);
            }
        }

        public int YearAttendanceCount
        {
            get
            {
                var startYear = new DateTime(SqlSingleton.sqlRepository.CurrentDateTime.Year, 1, 1);
                return UserAttendances.Count(p => p.AddedDate >= startYear);
            }
        }

        public int AllAttendanceCount
        {
            get
            {
                return UserAttendances.Count();
            }
        }
        #endregion

        public bool CanViewProfile(User user)
        {
            if (user != null)
            {
                if (user.ID == ID)
                {
                    return true;
                }
                //private
                if ((user.OwnTeam != null && user.OwnTeam.ID == PlayerOfTeamID) || (user.AssistantOfTeamID != null && user.AssistantOfTeamID == PlayerOfTeamID))
                {
                    return true;
                }
                //limited
                if (user != null && PublicLevel == (int)User.PublicLevelEnum.Limited) 
                {
                    return true;
                }
            }
            //public
            return PublicLevel == (int)User.PublicLevelEnum.Public;
        }

        public bool IsGradYear
        {
            get
            {
                return GradYearLevels.Any(p => p.ID == LevelID);
            }
        }

        public string PostAvatarPath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(AvatarPath))
                {
                    return AvatarPath;
                }
                if (!string.IsNullOrWhiteSpace(OwnTeam.LogoPath))
                {
                    return OwnTeam.LogoPath;
                }
                return "/Content/img/post_platform_logo.png";
            }
        }

        public bool InTestMode
        {
            get
            {
                return Mode == (int)User.ModeEnum.Test;
            }
        }
    }
}