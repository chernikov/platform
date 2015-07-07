using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Attributes.Validation;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class FeedbackView
    {

        public int ID { get; set; }

        [Required]
		public string Name {get; set; }

        [Required]
        [ValidEmail]
		public string Email {get; set; }

        [ValidPhone]
		public string Phone {get; set; }

        [Required]
		public string School {get; set; }

        [Required]
		public string City {get; set; }

		public int StateID {get; set; }

        [Required]
		public string Message {get; set; }

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

    }
}