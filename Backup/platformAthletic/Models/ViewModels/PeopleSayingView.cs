using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;
using System.ComponentModel.DataAnnotations;


namespace platformAthletic.Models.ViewModels
{
    public class PeopleSayingView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter Author")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Upload image 135x135")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Enter text")]
        public string Text { get; set; }

    }
}