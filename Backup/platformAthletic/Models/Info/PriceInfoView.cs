using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class PriceInfoView
    {
        [Required(ErrorMessage = "Enter a team subscription price")]
        public double TeamPrice { get; set; }

        [Required(ErrorMessage = "Enter a individual subscription price")]
        public double IndividualPrice { get; set; }

        public string TeamPriceImagePath { get; set; }

        public string IndividualPriceImagePath { get; set; }
    }
}