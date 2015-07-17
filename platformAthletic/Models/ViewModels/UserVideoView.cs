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
        [StringLength(500, ErrorMessage = "Header should not exceed 500 characters")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Enter link video")]
        [StringLength(500, ErrorMessage = "Video link should not exceed 500 characters")]
        public string VideoUrl { get; set; }

        public string VideoCode { get; set; }
       
    }
}