using System.ComponentModel.DataAnnotations;
using platformAthletic.Attributes.Validation;

namespace platformAthletic.Models.ViewModels.User
{
    public class ForgotPasswordView
    {
        [Required(ErrorMessage = "Enter Email")]
        [ValidEmail(ErrorMessage = "Enter a valid Email")]
        public string Email { get; set; }
    }
}