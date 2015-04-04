using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class RoleView
    {

        public int ID { get; set; }

		public string Code {get; set; }

		public string Name {get; set; }

    }
}