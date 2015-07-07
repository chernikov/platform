using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Helpers;

namespace platformAthletic.Models.ViewModels
{
    public class UserSeasonView
    {
        public int ID { get; set; }
        /// <summary>
        /// 1 - off season
        /// 2 - in season
        /// </summary>

        public int SeasonID { get; set; }

        public int? WeekID { get; set; }

        public int UserID { get; set; }

        public DateTime StartDay { get; set; }

        private IEnumerable<Season> Seasons
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Seasons.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListSeason
        {
            get
            {
                foreach (var season in Seasons)
                {
                    yield return new SelectListItem
                    {
                        Value = season.ID.ToString(),
                        Text = season.Name,
                        Selected = season.ID == SeasonID
                    };
                }
            }
        }


        private IEnumerable<Week> Weeks
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Weeks.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListWeek
        {
            get
            {
                yield return new SelectListItem {
                    Value = "",
                    Text = "None",
                    Selected = !WeekID.HasValue
                };

                foreach(var week in Weeks) 
                {
                    yield return new SelectListItem
                    {
                        Value = week.ID.ToString(),
                        Text = string.Format("{0} {1} {2} {3}", week.Phase.Cycle.Season.Name, week.Phase.Cycle.Name, week.Phase.Name, week.Name),
                        Selected = week.ID == WeekID
                    };
                }

            }
        }

        public void Init()
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            int numberOfWeek = (int)(((int)(DateTime.Now.Current() - StartDay).TotalDays + 7) / 7);
            var week = repository.Weeks.FirstOrDefault(p => p.Number == numberOfWeek && p.Phase.Cycle.SeasonID == SeasonID);
            if (week != null)
            {
                WeekID = week.ID;
            }
        }

    }
}