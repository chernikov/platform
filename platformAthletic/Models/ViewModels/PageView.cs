using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PageView
    {
        public int ID { get; set; }

		public string Name {get; set; }

		public string Text {get; set; }

    }
}