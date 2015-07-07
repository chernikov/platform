using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class StateView
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Enter name")]
		public string Name {get; set; }

        [Required(ErrorMessage = "Enter code")]
		public string Code {get; set; }

    }
}