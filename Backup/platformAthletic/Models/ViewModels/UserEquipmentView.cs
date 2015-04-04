using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class UserEquipmentView
    {

        public int ID { get; set; }

		public int UserID {get; set; }

		public int EquipmentID {get; set; }

    }
}