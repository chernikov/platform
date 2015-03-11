using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class CycleView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [DropDownField]
		public int SeasonID {get; set; }

        [TextBoxField]
        [ShowIndex]
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