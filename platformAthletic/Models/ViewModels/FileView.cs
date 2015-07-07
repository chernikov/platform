using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class FileView
    {

        public int ID { get; set; }

		public string Path {get; set; }

		public string Preview {get; set; }

		public DateTime AddedDate {get; set; }

    }
}