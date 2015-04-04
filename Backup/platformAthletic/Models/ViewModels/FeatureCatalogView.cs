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
	public class FeatureCatalogView
    {
        public int ID { get; set; }

        public int _ID
        {
            get
            {
                return ID;
            }
            set
            {
                ID = value;
            }
        }

        [Required(ErrorMessage="Enter Header")]
		public string Header {get; set; }
    }
}