using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class BannerPlaceView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [TextBoxField]
        [ShowIndex]
		public string Name {get; set; }

        [TextBoxField]
        [ShowIndex]
		public int Height {get; set; }

        [TextBoxField]
        [ShowIndex]
		public int Width {get; set; }

    }
}