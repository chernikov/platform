using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class CycleView
    {
        public int ID { get; set; }

		public int SeasonID {get; set; }

		public string Name {get; set; }

        private IEnumerable<Season> Seasons
        {
        	get 
            { 
                var repository = DependencyResolver.Current.GetService<IRepository>();
        	    return repository.Seasons.ToList();
        	}
        }
        
        public IEnumerable<SelectListItem> SelectListSeasonID
        {
        	get
        	{
        	    return Seasons.Select(p => new SelectListItem
        	                                        {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Name,
                                                        Selected = p.ID == SeasonID
        	                                        });
        	}
        }

    }
}