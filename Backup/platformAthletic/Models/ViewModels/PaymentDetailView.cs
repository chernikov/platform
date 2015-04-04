using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PaymentDetailView
    {
        [PrimaryField]
        public int ID { get; set; }

        [ShowIndex]
		public int UserID {get; set; }

        [ShowIndex]
		public double Amount {get; set; }

        [ShowIndex]
		public DateTime AddedDate {get; set; }

        [ShowIndex]
		public string Description {get; set; }

        [ShowIndex]
		public int? PromoCodeID {get; set; }

    }
}