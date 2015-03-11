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
	public class FieldPositionView
    {
        public int ID { get; set; }

        [Required]
        public string Code {get; set; }

        [Required]
        public string Name {get; set; }

    }
}