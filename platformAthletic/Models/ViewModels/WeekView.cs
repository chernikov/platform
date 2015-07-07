using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class WeekView
    {
        public int ID { get; set; }

        public int PhaseID {get; set; }

        [Required]
        public string Name { get; set; }

		public int? Number {get; set; }

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