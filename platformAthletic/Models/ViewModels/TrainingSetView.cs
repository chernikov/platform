using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class TrainingSetView
    {
        public int ID { get; set; }

        public List<TrainingEquipmentView> TrainingEquipments { get; set; }

        public TrainingSetView()
        {
            TrainingEquipments = new List<TrainingEquipmentView>();
        }

        public int DayID { get; set; }

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

        public int PhaseID { get; set; }

        private IEnumerable<Phase> Phases
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Phases.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListPhaseID
        {
            get
            {
                return Phases.Select(p => new SelectListItem
                                                    {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Name + " (" + p.Cycle.Season.Name + ")",
                                                        Selected = p.ID == PhaseID
                                                    });
            }
        }
    }
}