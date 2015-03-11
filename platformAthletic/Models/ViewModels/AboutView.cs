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
	public class AboutView
    {
     
        public int ID { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
     	public string Text {get; set; }

        
    }
}