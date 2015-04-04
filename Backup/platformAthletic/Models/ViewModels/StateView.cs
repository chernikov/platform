using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class StateView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [ShowIndex]
        [TextBoxField]
        [Required(ErrorMessage="Enter name")]
		public string Name {get; set; }

        [ShowIndex]
        [TextBoxField]
        [Required(ErrorMessage = "Enter code")]
		public string Code {get; set; }

    }
}