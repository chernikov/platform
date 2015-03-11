using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class FilterCustomProgressReport
    {
        public enum Type
        {
            Team = 0x01,
            Individual = 0x02,
            Position = 0x03,
            Group = 0x04
        }

        public enum Period
        {
            ByWeek = 0x01,
            ByMonth = 0x02
        }

        public enum WeightEnum
        {
            All = 0x01,
            Squat = 0x02,
            Bench = 0x03,
            Clean = 0x04,
        }

        public Type SelectedType { get; set; }

        public int TeamID { get; set; }

        public int? GroupID { get; set; }

        public int FieldPositionID { get; set; }

        public int UserID { get; set; }

        public Period SelectedPeriod { get; set; }

        public WeightEnum Weight { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable<SelectListItem> SelectListType
        {
            get
            {
                yield return new SelectListItem { Value = Type.Team.ToString(), Text = "Team", Selected = SelectedType == Type.Team };
                yield return new SelectListItem { Value = Type.Group.ToString(), Text = "Group", Selected = SelectedType == Type.Group };
                yield return new SelectListItem { Value = Type.Individual.ToString(), Text = "Individual", Selected = SelectedType == Type.Individual };
                yield return new SelectListItem { Value = Type.Position.ToString(), Text = "Position", Selected = SelectedType == Type.Position };
            }
        }


        public IEnumerable<Team> Teams
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Teams.Where(p => p.ID == TeamID).ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListTeams
        {
            get
            {
                return Teams.Select(p =>
                            new SelectListItem()
                            {
                                Value = p.ID.ToString(),
                                Text = p.Name,
                                Selected = TeamID == p.ID
                            }
                        );
            }
        }

        public IEnumerable<FieldPosition> FieldPositions
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.FieldPositions.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListFieldPositions
        {
            get
            {
                return FieldPositions.Select(p =>
                            new SelectListItem()
                            {
                                Value = p.ID.ToString(),
                                Text = p.Name,
                                Selected = FieldPositionID == p.ID
                            }
                        );
            }
        }

        public IEnumerable<User> Users
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Users.Where(p => p.PlayerOfTeamID == TeamID).ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListUsers
        {
            get
            {
                return Users.Select(p =>
                            new SelectListItem()
                            {
                                Value = p.ID.ToString(),
                                Text = p.FirstName + " " + p.LastName,
                                Selected = UserID == p.ID
                            }
                        );
            }
        }

        public IEnumerable<Group> Groups
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Groups.Where(p => p.TeamID == TeamID).ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListGroups
        {
            get
            {
                return Groups.Select(p =>
                            new SelectListItem()
                            {
                                Value = p.ID.ToString(),
                                Text = p.Name,
                                Selected = GroupID == p.ID
                            }
                        );
            }
        }


        public IEnumerable<SelectListItem> SelectListPeriod
        {
            get
            {
                yield return new SelectListItem { Value = Period.ByMonth.ToString(), Text = "By Month", Selected = SelectedPeriod == Period.ByMonth };
                yield return new SelectListItem { Value = Period.ByWeek.ToString(), Text = "By Week", Selected = SelectedPeriod == Period.ByWeek };
            }
        }

        public IEnumerable<SelectListItem> SelectListWeight
        {
            get
            {
                yield return new SelectListItem { Value = WeightEnum.All.ToString(), Text = WeightEnum.All.ToString(), Selected = Weight == WeightEnum.All };
                yield return new SelectListItem { Value = WeightEnum.Squat.ToString(), Text = WeightEnum.Squat.ToString(), Selected = Weight == WeightEnum.Squat };
                yield return new SelectListItem { Value = WeightEnum.Bench.ToString(), Text = WeightEnum.Bench.ToString(), Selected = Weight == WeightEnum.Bench };
                yield return new SelectListItem { Value = WeightEnum.Clean.ToString(), Text = WeightEnum.Clean.ToString(), Selected = Weight == WeightEnum.Clean };
            }
        }

        public FilterCustomProgressReport()
        {
            Weight = WeightEnum.All;
            SelectedPeriod = Period.ByMonth;
            SelectedType = Type.Team;
        }
    }
}