using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class EquipmentView
    {
        [PrimaryField]
        public int ID { get; set; }

        [ShowIndex]
        [TextBoxField]
		public string Name {get; set; }

        [ShowIndex]
		public string ImagePath {get; set; }
    }
}