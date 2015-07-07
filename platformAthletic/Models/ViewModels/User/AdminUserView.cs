using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Attributes.Validation;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels.User
{
    public class AdminUserView : BaseUserView
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter the password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}