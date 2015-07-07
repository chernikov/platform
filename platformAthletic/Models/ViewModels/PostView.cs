using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class PostView
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string TitleImagePath { get; set; }

        [Required]
        public string Header { get; set; }

        [Required]
        public string Text { get; set; }

        public bool Promoted { get; set; }

        public string VideoUrl { get; set; }

        public string VideoCode { get; set; }

    }
}