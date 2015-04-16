using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class SearchNationalLeaderBoard
    {
        public static int PageSize = 20;

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


        public enum SortEnum
        {
            NameAsc,
            NameDesc,
            SquatAsc,
            SquatDesc,
            BenchAsc,
            BenchDesc,
            CleanAsc,
            CleanDesc,
            TotalAsc,
            TotalDesc,
        }

        public int Page { get; set; }

        public SortEnum Sort { get; set; }

        public int? StateID { get; set; }

        public int? SportID { get; set; }

        public int? FieldPositionID { get; set; }

        public bool? Gender { get; set; }

        public int? Age { get; set; }

        public int? LevelID { get; set; }

        public bool ShowGradYear
        {
            get
            {
                return LevelID.HasValue && SearchNationalLeaderBoard.GradYearLevels.Any(p => p.ID == LevelID.Value);
            }
        }

        public int? GradYear { get; set; }

        public List<State> States {get; set; }
        
        public IEnumerable<SelectListItem> SelectListStateID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All States",
                    Selected = StateID == null
                };

                foreach (var state in States)
                {
                    yield return new SelectListItem
                    {
                        Value = state.ID.ToString(),
                        Text = state.Name,
                        Selected = state.ID == StateID
                    };
                }
            }
        }

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

        public List<FieldPosition> FieldPositions {get; set; }
        
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

        public List<bool> Genders { get; set; }

        public IEnumerable<SelectListItem> SelectListGender
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All Genders",
                    Selected = !Gender.HasValue
                };
                if (Genders.Contains(true))
                {
                    yield return new SelectListItem
                    {
                        Value = "true",
                        Text = "Male",
                        Selected = Gender.HasValue && Gender.Value
                    };
                }
                if (Genders.Contains(false))
                {
                    yield return new SelectListItem
                    {
                        Value = "false",
                        Text = "Female",
                        Selected = Gender.HasValue && !Gender.Value
                    };
                }
            }
        }
        public List<int> Ages { get; set; }

        public IEnumerable<SelectListItem> SelectListAge
        {
            get
            {
                
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All Ages",
                    Selected = Age == null
                };
                if (Ages.Contains(0))
                {
                    yield return new SelectListItem
                    {
                        Value = "0",
                        Text = "above 15",
                        Selected = Age == 0
                    };
                }
                for (int i = 15; i < 70; i = i + 5)
                {
                    if (Ages.Contains(i))
                    {
                        yield return new SelectListItem
                        {
                            Value = i.ToString(),
                            Text = i + " - " + (i + 4),
                            Selected = Age == i
                        };
                    }
                }
                if (Ages.Contains(70))
                {
                    yield return new SelectListItem
                    {
                        Value = "70",
                        Text = "older 70",
                        Selected = Age == 70
                    };
                }
            }
        }

        public List<Level> Levels { get; set; }

        public IEnumerable<SelectListItem> SelectListLevelID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All Levels",
                    Selected = LevelID == null
                };
                foreach (var level in Levels)
                {
                    yield return new SelectListItem
                    {
                        Value = level.ID.ToString(),
                        Text = level.Name,
                        Selected = level.ID == LevelID
                    };
                }
            }
        }

        public List<int> GradYears { get; set; }

        public IEnumerable<SelectListItem> SelectListGradYears
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All Years",
                    Selected = GradYear == null
                };
                foreach (var gradYear in GradYears)
                {
                    yield return new SelectListItem
                    {
                        Value = gradYear.ToString(),
                        Text = gradYear.ToString(),
                        Selected = gradYear == GradYear
                    };
                }
            }
        }

        public SearchNationalLeaderBoard()
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            Page = 1;
            Sort = SortEnum.TotalDesc;

            Sports = repository.Sports.ToList();

            Levels = repository.Levels.ToList();

            Ages = new List<int>() { 0, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70 };

            Genders = new List<bool>() { false, true };

            FieldPositions = repository.FieldPositions.ToList();

            States =  repository.States.ToList();
        }
    }
}