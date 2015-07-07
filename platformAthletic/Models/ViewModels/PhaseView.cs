using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PhaseView
    {
        
        public int ID { get; set; }
                
		public int CycleID {get; set; }
                
		public string Name {get; set; }

        private IEnumerable<Cycle> Cycles
        {
        	get 
            { 
                var repository = DependencyResolver.Current.GetService<IRepository>();
        	    return repository.Cycles.ToList();
        	}
        }
        
        public IEnumerable<SelectListItem> SelectListCycleID
        {
        	get
        	{
        	    return Cycles.Select(p => new SelectListItem
        	                                        {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Name + " (" + p.Season.Name + ")",
                                                        Selected = p.ID == CycleID
        	                                        });
        	}
        }

    }
}