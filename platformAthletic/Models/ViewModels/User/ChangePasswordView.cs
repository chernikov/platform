using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using platformAthletic.Attributes.Validation;

namespace platformAthletic.Models.ViewModels.User
{
    public class ChangePasswordView
    {
        public int ID { get; set; }

        [IsUserPassword(ErrorMessage = "Password is incorrect")]
        [Required(ErrorMessage = "Enter the password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter the new password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Enter the password again")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}