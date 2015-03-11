using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class FilterCustomAttendanceReport
    {
        public enum Type
        {
            Team = 0x01,
            Individual = 0x02,
            Position = 0x03,
            Group = 0x04,
        }

        public Type SelectedType { get; set; }

        public int TeamID { get; set; }

        public int? GroupID { get; set; }

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

        public FilterCustomAttendanceReport()
        {
            SelectedType = Type.Team;
        }
    }
}