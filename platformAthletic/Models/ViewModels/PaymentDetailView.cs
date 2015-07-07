using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PaymentDetailView
    {
        public int ID { get; set; }

		public int UserID {get; set; }

		public double Amount {get; set; }

		public DateTime AddedDate {get; set; }

		public string Description {get; set; }

		public int? PromoCodeID {get; set; }

    }
}