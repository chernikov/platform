using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class UserVideoView
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage="Enter Header")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Enter youtube link")]
        public string VideoUrl { get; set; }

        public string VideoCode { get; set; }
       
    }
}