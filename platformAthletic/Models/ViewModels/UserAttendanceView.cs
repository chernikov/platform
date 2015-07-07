using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class UserAttendanceView
    {

        public int ID { get; set; }

		public int TeamID {get; set; }

		public int UserID {get; set; }

		public DateTime AddedDate {get; set; }

    }
}