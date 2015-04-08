using platformAthletic.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.ViewModels.User
{
    public class PlayerView
    {
        [Required(ErrorMessage = "Enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter email")]
        [ValidEmail(ErrorMessage = "Enter correct email")]
        [UserEmailValidation(ErrorMessage = "Email already registered")]
        public string Email { get; set; }
    }
}