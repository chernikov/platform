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
	public class FaqView
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Enter Header")]
        public string Header {get; set; }

        [Required(ErrorMessage="Enter text")]
		public string Text {get; set; }
    }
}