using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class FilterTrainingDayInfo
    {
        public int? WeekID { get; set; }

        public IEnumerable<Week> Weeks
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                if (PhaseID.HasValue)
                {
                    return repository.Weeks.Where(p => p.PhaseID == PhaseID).ToList();
                }
                return null;
                
            }
        }

        public IEnumerable<SelectListItem> SelectListWeeks
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "All weeks",
                    Selected = !WeekID.HasValue
                };
                if (Weeks != null)
                {
                    foreach (var week in Weeks)
                    {
                        yield return new SelectListItem()
                        {
                            Value = week.ID.ToString(),
                            Text = week.Name,
                            Selected = WeekID == week.ID
                        };
                    }
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
