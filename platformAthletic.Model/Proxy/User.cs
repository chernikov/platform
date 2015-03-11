using System;
using System.Collections.Generic;
using System.Linq;
using ManageAttribute;

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
                else if (Team != null)
                {
                    return Team.PrimaryColor ?? string.Empty;
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
                else if (Team != null)
                {
                    return Team.SecondaryColor ?? string.Empty;
                }
                return string.Empty;
            }
        }

        public UserSeason CurrentSeason
        {
            get
            {
                if (Team != null && ID != Team.UserID)
                {
                    return Team.CurrentSeason;
                }
                else
                {
                    var season = UserSeasons.OrderByDescending(p => p.StartDay).ThenByDescending(p => ID).FirstOrDefault(p => p.StartDay <= DateTime.Now);

                    if (season == null)
                    {
                        season = UserSeasons.Where(p => p.StartDay >= DateTime.Now).OrderBy(p => p.StartDay).ThenByDescending(p => ID).FirstOrDefault();
                    }
                    return season;
                }
            }
        }

        public UserSeason SeasonByDate(DateTime dateTime)
        {
            if (Team != null && ID != Team.UserID)
            {
                return Team.User.SeasonByDate(dateTime);
            }
            else
            {
                var season = UserSeasons.OrderByDescending(p => p.StartDay).ThenByDescending(p => ID).FirstOrDefault(p => p.StartDay <= dateTime);
                if (season == null)
                {
                    season = UserSeasons.Where(p => p.StartDay >= dateTime).OrderBy(p => p.StartDay).ThenByDescending(p => ID).FirstOrDefault();
                }
                return season;
            }
            
        }

        public UserSeason NextSeason
        {
            get
            {
                if (Team != null && ID != Team.UserID)
                {
                    return Team.NextSeason;
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
                if (Team != null)
                {
                    return Team.LogoPath;
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
                    return CurrentSeason.StartDay <= DateTime.Now &&
                        CurrentSeason.StartDay.AddDays(CurrentSeason.Season.DaysLength) >= DateTime.Now;
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
                    return NextSeason.StartDay <= DateTime.Now &&
                    NextSeason.StartDay.AddDays(NextSeason.Season.DaysLength) >= DateTime.Now;
                }
                return false;
            }
        }

        public bool HasAttendanceToday
        {
            get
            {
                return UserAttendances.Any(p => p.AddedDate == DateTime.Now.Date);
            }
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
                return OwnTeam.Users.Any();
            }
        }

        public bool CanEditTeamData(User user)
        {
            if (user != null)
            {
                if (user.OwnTeam != null && user.OwnTeam.ID == PlayerOfTeamID)
                {
                    return true;
                }
                if (InRoles("individual") && user.ID == ID)
                {
                    return true;
                }
                if (user.Team != null && user.Team.SBCControl == (int)Team.SBCControlType.CoachAndPlayer && user.ID == ID)
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
            }
            return false;
        }

        public List<Equipment> Equpments
        {
            get
            {
                if (InRoles("player"))
                {
                    return Team.User.UserEquipments.Select(p => p.Equipment).ToList();
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
                    return Team.User.PaidTill;
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
                    int numberOfWeek = (int)(((int)((DateTime.Now - CurrentSeason.StartDay).TotalDays) / 7));
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
    }
}