using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class BannerPlaceView
    {
        public int ID { get; set; }

        public string Name {get; set; }

        public int Height {get; set; }

        public int Width {get; set; }
    }
}