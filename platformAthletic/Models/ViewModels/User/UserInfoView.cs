using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.ViewModels.User
{
    public class UserInfoView
    {
       

        public class SportInfo
        {
            private IRepository repository = DependencyResolver.Current.GetService<IRepository>();

            public int Num { get; set; }

            public int? SportID { get; set; }

            public int? FieldPositionID { get; set; }

            private List<Sport> Sports
            {
                get
                {
                    return repository.Sports.ToList();
                }
            }

            public IEnumerable<SelectListItem> SelectListSports
            {
                get
                {
                    yield return new SelectListItem()
                    {
                        Value = "",
                        Text = "Not selected",
                        Selected = !SportID.HasValue
                    };

                    foreach (var sport in Sports)
                    {
                        yield return new SelectListItem()
                        {
                            Value = sport.ID.ToString(),
                            Text = sport.Name,
                            Selected = SportID == sport.ID
                        };
                    }
                }
            }

            public List<FieldPosition> FieldPositions
            {
                get
                {
                    return repository.FieldPositions.ToList();
                }
            }

            public IEnumerable<SelectListItem> SelectListFieldPositionID
            {
                get
                {
                    yield return new SelectListItem
                    {
                        Value = "",
                        Text = "Not selected",
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
        }

        private IRepository repository = DependencyResolver.Current.GetService<IRepository>();

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


        public int ID { get; set; }

        public string TeamSchoolName { get; set; }

        public string TeamStateName { get; set; }

        public string FullAvatarPath { get; set; }

        public string AvatarPath { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public bool Gender { get; set; }

        public int GradYear { get; set; }

        public bool IsGradYear
        {
            get
            {
                return GradYearLevels.Any(p => p.ID == LevelID);
            }
        }

        public IEnumerable<SelectListItem> SelectListGender
        {
            get
            {
                yield return new SelectListItem()
                {
                    Text = "Male",
                    Value = "True",
                    Selected = Gender
                };

                yield return new SelectListItem()
                {
                    Text = "Female",
                    Value = "False",
                    Selected = !Gender
                };
            }
        }

        public List<SportInfo> Sports { get; set; }

        private List<Level> Levels
        {
            get
            {
                return repository.Levels.ToList();
            }
        }

        public int LevelID { get; set; }

        public IEnumerable<SelectListItem> SelectListLevel
        {
            get
            {
                foreach (var level in Levels)
                {
                    yield return new SelectListItem()
                    {
                        Value = level.ID.ToString(),
                        Text = level.Name,
                        Selected = LevelID == level.ID
                    };
                }
            }
        }

        public int Squat { get; set; }

        public int Bench { get; set; }

        public int Clean { get; set; }

        public string Height { get; set; }

        public string _40YardDash { get; set; }

        public string Weight { get; set; }

        public string Vertical { get; set; }

        public UserInfoView()
        {
            Sports = new List<SportInfo>() { new SportInfo() { Num = 0 }, new SportInfo() { Num = 1 }, new SportInfo() { Num = 2 } };
        }
    }
}