using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Attributes.Validation;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class InvoiceView
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage="Enter Name of Organization")]
		public string NameOfOrganization {get; set; }

        [Required(ErrorMessage = "Enter City")]
		public string City {get; set; }

		public int StateID {get; set; }

        [Required(ErrorMessage = "Enter Zip")]
		public string ZipCode {get; set; }

        [Required(ErrorMessage="Enter Phone Number")]
        [ValidPhone(ErrorMessage ="Enter Correct Phone Number")]
		public string PhoneNumber {get; set; }

		public string ReferralCode {get; set; }

		public DateTime DateSent {get; set; }

		public DateTime DateDue {get; set; }

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