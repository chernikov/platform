using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;

namespace platformAthletic.Models.ViewModels.User
{
    public class PlayerUserInfoView
    {
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

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool Gender { get; set; }

        public int? GradYear { get; set; }

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

        public IEnumerable<SelectListItem> SelectListGradYear
        {
            get
            {
                yield return new SelectListItem() { Value = "", Text = "Not selected", Selected = !GradYear.HasValue };
                for (var i = DateTime.Now.Current().Year + 9; i >= DateTime.Now.Current().Year; i--)
                {
                    yield return new SelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = GradYear == i };
                }
            }
        }

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

        public int BirthdayDay { get; set; }

        public int BirthdayMonth { get; set; }

        public int BirthdayYear { get; set; }

        public IEnumerable<SelectListItem> BirthdayDaySelectList
        {
            get
            {
                for (int i = 1; i < 32; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = BirthdayDay == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> BirthdayMonthSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(2000, i, 1).ToString("MMMM"),
                        Selected = BirthdayMonth == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> BirthdayYearSelectList
        {
            get
            {
                for (int i = 1910; i < DateTime.Now.Year; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = BirthdayYear == i
                    };
                }
            }
        }
    }
}