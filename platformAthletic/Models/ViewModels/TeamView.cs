using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class TeamView
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Enter a Team Name")]
		public string Name {get; set; }

		public string LogoPath {get; set; }

        public int StateID {get; set; }

		public string PrimaryColor {get; set; }

		public string SecondaryColor {get; set; }

        private IEnumerable<State> States
        {
        	get 
            { 
                var repository = DependencyResolver.Current.GetService<IRepository>();
        	    return repository.States.ToList();
        	}
        }
        
        public IEnumerable<SelectListItem> SelectListStateID
        {
        	get
        	{
        	    return States.Select(p => new SelectListItem
        	                                        {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Name,
                                                        Selected = p.ID == StateID
        	                                        });
        	}
        }

        public int MaxCount { get; set; }

        public IEnumerable<SelectListItem> SelectListMaxCount
        {
            get
            {
                for (var i = 50; i <= 1000; i = i + 50)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = MaxCount == i
                    };
                }
            }
        }
    }
}