using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using System.ComponentModel.DataAnnotations;


namespace platformAthletic.Models.ViewModels
{ 
	public class GroupView
    {
        public int ID { get; set; }
		
        [Required(ErrorMessage="Enter name")]
        [StringLength(500, ErrorMessage = "The length of the name should not exceed 500 characters")]
		public string Name {get; set; }

    }
}