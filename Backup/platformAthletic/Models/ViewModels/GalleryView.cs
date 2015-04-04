using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class GalleryView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [ShowIndex]
        [HiddenField]    
		public string ImagePath {get; set; }
    }
}