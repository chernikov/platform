using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class SBCValueView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [HiddenField]
		public int UserID {get; set; }

        [ShowIndex]
		public DateTime AddedDate {get; set; }

        [TextBoxField]
		public double Squat {get; set; }

        [TextBoxField]
		public double Bench {get; set; }

        [TextBoxField]
		public double Clean {get; set; }

        [TextBoxField]
        public string FirstName { get; set; }

        [TextBoxField]
        public string LastName { get; set; }
    }
}