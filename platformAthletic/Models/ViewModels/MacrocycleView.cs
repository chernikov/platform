using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class MacrocycleView
    {

        public int ID { get; set; }

        public int SeasonType { get; set; }

        public int WeekID { get; set; }

        public string Name { get; set; }

        private Season Season
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Seasons.FirstOrDefault(p => p.Type == SeasonType);
            }
        }

        private IEnumerable<Week> Weeks
        {
            get
            {
                if (Season != null)
                {
                    var weeks = Season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks);
                    weeks = weeks.Where(p => p.TrainingDays.Any(r => r.MacrocycleID == null));
                    return weeks;
                }
                return null;
            }
        }

        public IEnumerable<SelectListItem> SelectListWeeks
        {
            get
            {
                if (Season != null)
                {
                    foreach (var week in Weeks)
                    {
                        yield return new SelectListItem()
                        {
                            Value = week.ID.ToString(),
                            Text = string.Format("[{0}] {1}", week.Number.HasValue ? week.Number.ToString() : "no number" , week.Name),
                            Selected = WeekID == week.ID
                        };
                    }
                }
            }
        }
    }
}