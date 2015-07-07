using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class FeatureTextView
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

        
		public int FeatureCatalogID {get; set; }

        [Required(ErrorMessage="Enter a Header")]
 		public string Header {get; set; }

        [Required(ErrorMessage = "Enter text")]
 		public string Text {get; set; }
    }
}