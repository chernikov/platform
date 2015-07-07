using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class SBCValueView
    {
        public int ID { get; set; }

		public int UserID {get; set; }

        public DateTime AddedDate {get; set; }

		public double Squat {get; set; }

		public double Bench {get; set; }

		public double Clean {get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}