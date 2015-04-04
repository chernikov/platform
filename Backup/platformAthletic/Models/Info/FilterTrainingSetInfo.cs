using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class FilterTrainingSetInfo
    {
        public int? DayID { get; set; }

        public IEnumerable<Day> Days
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Days.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListDays
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "All days",
                    Selected = !DayID.HasValue
                };
                foreach (var day in Days)
                {
                    yield return new SelectListItem()
                    {
                        Value = day.ID.ToString(),
                        Text = day.Name,
                        Selected = DayID == day.ID
                    };
                }
            }
        }

        public int? PhaseID { get; set; }

        public IEnumerable<Phase> Phases
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Phases.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListPhases
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "All phases",
                    Selected = !PhaseID.HasValue
                };
                foreach (var phase in Phases)
                {
                    yield return new SelectListItem()
                    {
                        Value = phase.ID.ToString(),
                        Text = phase.Cycle.Season.Name + " " + phase.Name,
                        Selected = PhaseID == phase.ID
                    };
                }
            }
        }
    }
}