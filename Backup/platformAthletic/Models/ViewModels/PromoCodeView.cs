using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PromoCodeView
    {
        public int ID { get; set; }

		public int PromoActionID {get; set; }

        [Required(ErrorMessage="¬ведите количество")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "¬ведите код")]
		public string Code {get; set; }

		public bool Reusable {get; set; }

    }
}