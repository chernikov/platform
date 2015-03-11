using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class TrainingDayView
    {
        public int ID { get; set; }

        public int WeekPhaseCycleSeasonType { get; set; }

        public int WeekID { get; set; }

        public int WeekPhaseID { get; set; }

        public int DayID { get; set; }

        public string DayName { get; set; }

        public string WeekFullName { get; set; }

        private IEnumerable<Day> Days
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Days.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListDayID
        {
            get
            {
                return Days.Select(p => new SelectListItem
                                                    {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Name,
                                                        Selected = p.ID == DayID
                                                    });
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

        public IEnumerable<SelectListItem> SelectListWeekID
        {
            get
            {
                return Weeks.Select(p => new SelectListItem
                                                    {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Phase.Cycle.Season.Name + " " + p.Phase.Cycle.Name + " " + p.Phase.Name + " " + p.Name,
                                                        Selected = p.ID == WeekID
                                                    });
            }
        }
        public List<TrainingDayCell> TrainingDayCells { get; set; }

        public MvcHtmlString GetCellInfo(int id)
        {
            
            var cell = TrainingDayCells.FirstOrDefault(p => p.CellID == id);
            if (cell != null)
            {
                return new MvcHtmlString(cell.Value);
            }
            return new MvcHtmlString("&nbsp;");
            /*return new MvcHtmlString("val" + id.ToString());*/
        }

        public string Batch { get; set; }
    }
}