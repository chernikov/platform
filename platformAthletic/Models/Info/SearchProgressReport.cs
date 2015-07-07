using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    public class SearchProgressReport
    {

        public enum SortEnum
        {
            SquatAsc,
            SquatDesc,
            BenchAsc,
            BenchDesc,
            CleanAsc,
            CleanDesc,
            TotalAsc,
            TotalDesc,
        }

        protected IRepository Repository = DependencyResolver.Current.GetService<IRepository>();


        public int TeamID { get; set; }

        public int Page { get; set; }

        public SortEnum Sort { get; set; }

        public int? SportID { get; set; }

        public int? FieldPositionID { get; set; }

        public int? GroupID { get; set; }

        public int? GradYear { get; set; }

        public DateTime? StartPeriod { get; set; }

        public DateTime? EndPeriod { get; set; }

        public string SearchString { get; set; }

        public int? StartID { get; set; }

        public List<Sport> Sports { get; set; }

        public IEnumerable<SelectListItem> SelectListSportID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All Sports",
                    Selected = SportID == null
                };

                foreach (var sport in Sports)
                {
                    yield return new SelectListItem
                    {
                        Value = sport.ID.ToString(),
                        Text = sport.Name,
                        Selected = sport.ID == SportID
                    };
                }
            }
        }

        public List<FieldPosition> FieldPositions { get; set; }

        public IEnumerable<SelectListItem> SelectListFieldPositionID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All Positions",
                    Selected = FieldPositionID == null
                };
                if (SportID != null)
                {
                    var positions = FieldPositions.Where(p => p.SportID == SportID.Value).ToList();
                    foreach (var position in positions)
                    {
                        yield return new SelectListItem
                        {
                            Value = position.ID.ToString(),
                            Text = position.Name,
                            Selected = position.ID == FieldPositionID
                        };
                    }
                }
            }
        }

        public List<Group> Groups { get; set; }

        public IEnumerable<SelectListItem> SelectListGroupID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All Groups",
                    Selected = GroupID == null
                };
                if (Groups != null)
                {
                    foreach (var group in Groups)
                    {
                        yield return new SelectListItem
                        {
                            Value = group.ID.ToString(),
                            Text = group.Name,
                            Selected = group.ID == GroupID
                        };
                    }
                }
            }
        }

        public List<int> GradYears { get; set; }

        public IEnumerable<SelectListItem> SelectListGradYear
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All Years",
                    Selected = GradYear == null
                };
                if (GradYears != null)
                {
                    foreach (var gradYears in GradYears)
                    {
                        yield return new SelectListItem
                        {
                            Value = gradYears.ToString(),
                            Text = gradYears.ToString(),
                            Selected = gradYears == GradYear
                        };
                    }
                }
            }
        }

        public bool IsDateFilter
        {
            get
            {
                return StartPeriod.HasValue || EndPeriod.HasValue;
            }
        }
        public SearchProgressReport()
        {
            Sort = SortEnum.TotalDesc;
            Page = 1;
        }

        public void Init()
        {
            Sports = Repository.Sports.ToList();
            FieldPositions = Repository.FieldPositions.ToList();
            Groups = Repository.Groups.Where(p => p.TeamID == TeamID).ToList();
            GradYears = Repository.TeamPlayersUsers.Where(p => p.PlayerOfTeamID == TeamID && p.GradYear.HasValue).Select(p => p.GradYear.Value).Distinct().ToList();
            
            
        }
    }
}